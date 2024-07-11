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
public class CompShipExoticCrucible : CompShipHeatSink
{
    private Effecter _progressBarEffecter;

    /// <summary>
    ///     The amount of work left to complete the reaction
    /// </summary>
    public float ReactionWorkLeft;

    /// <summary>
    ///     Whether the reaction can occur with the current heat, power, and exotic crucible state
    /// </summary>
    public bool CanReact => heatStored >= Props.reactionMinimumHeat && PowerTrader.PowerOn && !Disabled;

    /// <summary>
    ///     The speed of the reaction, adjusted by heat and power
    /// </summary>
    public float ReactionSpeed
    {
        get
        {
            if (!CanReact) return 0;

            var speed = Props.reactionSpeedBase;

            speed *= ExoticCrucibleSettings.globalReactionSpeedMultiplier;

#if DEBUG
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
    ///     The power trader component
    /// </summary>
    public CompPowerTrader PowerTrader => (CompPowerTrader)powerComp;

    /// <summary>
    ///     The properties of the exotic crucible
    /// </summary>
    public new CompProps_ShipExoticCrucible Props => (CompProps_ShipExoticCrucible)props;

    /// <summary>
    ///     Expose data to save/load
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref ReactionWorkLeft, "reactionWorkLeft");
    }

    /// <summary>
    ///     Tick the component
    /// </summary>
    public override void CompTick()
    {
        base.CompTick();

        // update power consumption every 60 ticks
        if (!parent.IsHashIntervalTick(60)) return;

        PowerTrader.PowerOutput = CanReact ? -powerComp.Props.PowerConsumption : -powerComp.Props.idlePowerDraw;

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

        // reduce work left by the reaction speed
        ReactionWorkLeft -= ReactionSpeed;

        // update the progress bar effect
        if (CanReact)
        {
            _progressBarEffecter ??= EffecterDefOf.ProgressBar.Spawn();
            _progressBarEffecter.EffectTick(parent, TargetInfo.Invalid);
            var mote = ((SubEffecter_ProgressBar)_progressBarEffecter.children[0]).mote;
            mote.progress = 1f - ReactionWorkLeft / Props.reactionWorkAmount;
#if DEBUG
            mote.offsetZ = progressBarOffsetZ;
#else
            mote.offsetZ = -0.9f;
#endif
        }
        else
        {
            _progressBarEffecter?.Cleanup();
            _progressBarEffecter = null;
        }
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

    [TweakValue("ExoticCrucible", -5, 5)]
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    public static float progressBarOffsetZ = -0.9f;
#endif
}
