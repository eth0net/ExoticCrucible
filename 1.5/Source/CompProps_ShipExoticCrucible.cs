using System.Diagnostics.CodeAnalysis;
using SaveOurShip2;
using Verse;

namespace ExoticCrucible;

/// <summary>
///     The component properties for the exotic crucible
/// </summary>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
public class CompProps_ShipExoticCrucible : CompProps_ShipHeat
{
    /// <summary>
    ///     The multiplier applied to the reaction speed based on the heat over the minimum
    /// </summary>
    public float reactionHeatBonus = 1f;

    /// <summary>
    ///     The minimum heat required for the reaction to occur
    /// </summary>
    public float reactionMinimumHeat = 100f;

    /// <summary>
    ///     The product of the reaction
    /// </summary>
    public ThingDef reactionProduct = null;

    /// <summary>
    ///     The amount of product to spawn
    /// </summary>
    public int reactionProductAmount = 1;

    /// <summary>
    ///     The amount of work done per tick, before bonuses
    /// </summary>
    public float reactionSpeedBase = 1f;

    /// <summary>
    ///     The amount of work required to complete the reaction
    /// </summary>
    public float reactionWorkAmount = 10000f;
}
