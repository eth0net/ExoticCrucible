using RimWorld;
using SaveOurShip2;
using Verse;

namespace ExoticHeatsink;

[StaticConstructorOnStartup]
public class CompShipHeatSink_Exotic : CompShipHeatSink
{
    /// <summary>
    /// The amount of work left to complete the reaction
    /// </summary>
    public float reactionWorkLeft;

    /// <summary>
    /// Whether the reaction can occur with the current heat and power state
    /// </summary>
    public bool CanReact => heatStored >= Props.reactionMinimumHeat && PowerTrader.PowerOn;

    /// <summary>
    /// The speed of the reaction, adjusted by heat and power
    /// </summary>
    public float ReactionSpeed
    {
        get
        {
            if (!CanReact) return 0;

            var speed = Props.reactionSpeedBase;

            // apply heat bonus, adjusted by the bonus multiplier
            if (heatStored > Props.reactionMinimumHeat)
            {
                // minimum of 1x bonus multiplier
                speed *= (heatStored - Props.reactionMinimumHeat) * Props.reactionHeatBonus;
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
    /// Tick the component, 
    /// </summary>
    public override void CompTick()
    {
        base.CompTick();

        // reduce work left by the reaction speed
        reactionWorkLeft -= ReactionSpeed;

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
    }
}
