using SaveOurShip2;
using Verse;

namespace ExoticHeatsink;

/// <summary>
/// The component properties for the exotic heatsink
/// </summary>
public class CompProps_ShipHeatSinkExotic : CompProps_ShipHeat
{
    /// <summary>
    /// The amount of work required to complete the reaction
    /// </summary>
    public float reactionWorkAmount;

    /// <summary>
    /// The amount of work done per tick, before bonuses
    /// </summary>
    public float reactionSpeedBase = 1f;

    /// <summary>
    /// The minimum heat required for the reaction to occur
    /// </summary>
    public float reactionMinimumHeat;

    /// <summary>
    /// The multiplier applied to the reaction speed based on the heat over the minimum
    /// </summary>
    public float reactionHeatBonus = 1f;

    /// <summary>
    /// The product of the reaction
    /// </summary>
    public ThingDef reactionProduct;

    /// <summary>
    /// The amount of product to spawn
    /// </summary>
    public int reactionProductAmount = 1;
}

