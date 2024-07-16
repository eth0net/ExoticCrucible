using System.Diagnostics.CodeAnalysis;
using RimWorld;
using SaveOurShip2;
using Verse;
#if DEBUG
using LudeonTK;
#endif

namespace ExoticCrucible;

/// <summary>
///     The component implementation for the exotic crucible
/// </summary>
[StaticConstructorOnStartup]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CompShipExoticCrucible : CompShipHeatSink
{
    private Effecter _progressBarEffecter;

    /// <summary>
    ///     How frequently to update the reaction.
    /// </summary>
    private const int TickInterval = 60;

    /// <summary>
    ///     The amount of work left to complete the reaction.
    /// </summary>
    public float ReactionWorkLeft;

    /// <summary>
    ///     Whether the reaction can occur with the current state.
    /// </summary>
    public bool CanReact => (!ExoticCrucibleSettings.reactionRequiresHeat || heatStored >= Props.reactionMinimumHeat) &&
                            PowerTrader.PowerOn && !Disabled;

    /// <summary>
    ///     The speed of the reaction including various modifiers.
    /// </summary>
    public float ReactionSpeed
    {
        get
        {
            var speed = Props.reactionSpeedBase;

            // apply the tick interval to account for the fact that we don't check every tick
            speed *= TickInterval;

            // apply the global speed multiplier
            speed *= ExoticCrucibleSettings.globalReactionSpeedMultiplier;

#if DEBUG
            // apply the tweak speed multiplier
            speed *= tweakReactionSpeedMultiplier;
#endif

            // if heat is above the minimum, apply the heat bonus
            if (heatStored < Props.reactionMinimumHeat) return speed;

            speed *= heatStored - Props.reactionMinimumHeat;
            speed *= Props.reactionHeatBonus;
            speed *= ExoticCrucibleSettings.globalReactionHeatBonusMultiplier;
#if DEBUG
            speed *= tweakReactionHeatBonusMultiplier;
#endif

            return speed;
        }
    }

    /// <summary>
    ///     The power trader component.
    /// </summary>
    public CompPowerTrader PowerTrader => (CompPowerTrader)powerComp;

    /// <summary>
    ///     The properties of the exotic crucible.
    /// </summary>
    public new CompProps_ShipExoticCrucible Props => (CompProps_ShipExoticCrucible)props;

    /// <summary>
    ///     Expose data to save/load.
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref ReactionWorkLeft, "reactionWorkLeft");
    }

    /// <summary>
    ///     Post spawn setup.
    /// </summary>
    /// <param name="respawningAfterLoad"></param>
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);
        if (respawningAfterLoad) return;
        ReactionWorkLeft = Props.reactionWorkAmount;
    }

    /// <summary>
    ///     Tick the component.
    /// </summary>
    public override void CompTick()
    {
        base.CompTick();

        // only check every 60 ticks
        if (!parent.IsHashIntervalTick(TickInterval)) return;

        // skip the tick if we can't react
        if (!CanReact)
        {
            // set power consumption to idle
            PowerTrader.PowerOutput = -PowerTrader.Props.idlePowerDraw;

            // cleanup the progress bar effect
            _progressBarEffecter?.Cleanup();
            _progressBarEffecter = null;

            return;
        }

        // set power consumption to the reaction power curve if we have enough heat or idle if not
        // speed is calculated with or without heat bonus independently in ReactionSpeed
        PowerTrader.PowerOutput = heatStored >= Props.reactionMinimumHeat
            ? -Props.reactionHeatPowerCurve.Evaluate(heatStored)
            : -PowerTrader.Props.idlePowerDraw;

        // reduce work left by the reaction speed
        ReactionWorkLeft -= ReactionSpeed;

        // check if work is done
        if (ReactionWorkLeft <= 0)
        {
            // spawn reaction product
            var thing = ThingMaker.MakeThing(Props.reactionProduct);
            thing.stackCount = Props.reactionProductAmount;
            GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);

            // reset work left
            ReactionWorkLeft += Props.reactionWorkAmount;
        }

        // update the progress bar effect
        _progressBarEffecter ??= EffecterDefOf.ProgressBar.Spawn();
        _progressBarEffecter.EffectTick(parent, TargetInfo.Invalid);
        var mote = ((SubEffecter_ProgressBar)_progressBarEffecter.children[0]).mote;
        mote.progress = 1f - ReactionWorkLeft / Props.reactionWorkAmount;
        mote.offsetZ = -0.9f;
    }

#if DEBUG
    [TweakValue("ExoticCrucible", 0.001f, 1000f)]
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    public static float tweakReactionSpeedMultiplier = 1f;

    [TweakValue("ExoticCrucible", 0.001f, 1000f)]
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    public static float tweakReactionHeatBonusMultiplier = 1f;
#endif
}
