using RimWorld;
using SaveOurShip2;
using Verse;

namespace ExoticHeatsink;

/// <summary>
/// The component implementation for the exotic heatsink
/// </summary>
[StaticConstructorOnStartup]
public class CompShipHeatSink_Exotic : CompShipHeatSink
{
#if DEBUG
    [TweakValue("ExoticHeatsink", 0.001f, 1000f)]
    public static float tweakReactionSpeedMultiplier = 1f;

    [TweakValue("ExoticHeatsink", 0.001f, 1000f)]
    public static float tweakReactionHeatBonusMultiplier = 1f;

    [TweakValue("ExoticHeatsink", -5, 5)]
    public static float progressBarOffsetZ = -0.9f;
#endif

    private Effecter progressBarEffecter;

    /// <summary>
    /// The amount of work left to complete the reaction
    /// </summary>
    public float reactionWorkLeft;

    /// <summary>
    /// Whether the reaction can occur with the current heat, power, and heatsink state
    /// </summary>
    public bool CanReact => heatStored >= Props.reactionMinimumHeat && PowerTrader.PowerOn && !Disabled;

    /// <summary>
    /// The speed of the reaction, adjusted by heat and power
    /// </summary>
    public float ReactionSpeed
    {
        get
        {
            if (!CanReact) return 0;

            var speed = Props.reactionSpeedBase;

            speed *= ExoticHeatsinkSettings.globalReactionSpeedMultiplier;

#if DEBUG
            speed *= tweakReactionSpeedMultiplier;
#endif

            // if heat is above the minimum, apply the heat bonus
            if (heatStored > Props.reactionMinimumHeat)
            {
                speed *= heatStored - Props.reactionMinimumHeat;
                speed *= Props.reactionHeatBonus;
                speed *= ExoticHeatsinkSettings.globalReactionHeatBonusMultiplier;
#if DEBUG
                speed *= tweakReactionHeatBonusMultiplier;
#endif
            }

            return speed;
        }
    }

    /// <summary>
    /// The power trader component
    /// </summary>
    public CompPowerTrader PowerTrader => (CompPowerTrader)powerComp;

    /// <summary>
    /// The properties of the exotic heat sink
    /// </summary>
    public new CompProps_ShipHeatSinkExotic Props => (CompProps_ShipHeatSinkExotic)props;

    /// <summary>
    /// Expose data to save/load
    /// </summary>
    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref reactionWorkLeft, "reactionWorkLeft");
    }

    /// <summary>
    /// Tick the component
    /// </summary>
    public override void CompTick()
    {
        base.CompTick();

        // update power consumption every 60 ticks
        if (parent.IsHashIntervalTick(60))
        {
            PowerTrader.PowerOutput = CanReact ? -powerComp.Props.PowerConsumption : -powerComp.Props.idlePowerDraw;
        }

        // check if work is done
        if (reactionWorkLeft <= 0)
        {
            // spawn reaction product
            var thing = ThingMaker.MakeThing(Props.reactionProduct);
            thing.stackCount = Props.reactionProductAmount;
            GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);

            // reset work left
            reactionWorkLeft += Props.reactionWorkAmount;
        }

        // reduce work left by the reaction speed
        reactionWorkLeft -= ReactionSpeed;

        // update the progress bar effect
        if (CanReact)
        {
            progressBarEffecter ??= EffecterDefOf.ProgressBar.Spawn();
            progressBarEffecter.EffectTick(parent, TargetInfo.Invalid);
            var mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
            mote.progress = 1f - reactionWorkLeft / Props.reactionWorkAmount;
#if DEBUG
            mote.offsetZ = progressBarOffsetZ;
#else
            mote.offsetZ = -0.9f;
#endif
        }
        else
        {
            progressBarEffecter?.Cleanup();
            progressBarEffecter = null;
        }
    }
}
