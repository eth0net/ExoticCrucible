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
    ///     The multiplier applied to the reaction speed based on the heat over the minimum.
    ///     1.0 means all heat over the minimum is applied to the speed.
    ///     0.5 means half the heat over the minimum is applied to the speed.
    /// </summary>
    public float reactionHeatBonus = 1f;

    /// <summary>
    ///     The curve that determines the power required for the reaction to occur.
    ///     X is the stored heat, Y is the resulting power draw.
    ///     Note that this only takes effect if the heat is above the minimum required.
    ///     If the heat is below the minimum, the power draw is set to idle.
    /// </summary>
    public SimpleCurve reactionHeatPowerCurve = new([
        // new CurvePoint(-5000, -22500),
        // new CurvePoint(500, 7500),
        // new CurvePoint(6000, 37500)
        new CurvePoint(1, 1),
        new CurvePoint(2, 1),
        new CurvePoint(3, 1)
    ]);

    /// <summary>
    ///     The minimum heat required for the reaction.
    ///     Depending on the active settings, this may prevent the reaction
    ///     from occurring below this limit, or it may just prevent the heat
    ///     bonus from being applied.
    /// </summary>
    public float reactionMinimumHeat = 100f;

    /// <summary>
    ///     The product of the reaction when it completes.
    /// </summary>
    public ThingDef reactionProduct = null;

    /// <summary>
    ///     The amount of product to spawn when the reaction completes.
    /// </summary>
    public int reactionProductAmount = 1;

    /// <summary>
    ///     The amount of work done per tick, before bonuses.
    /// </summary>
    public float reactionSpeedBase = 1f;

    /// <summary>
    ///     The amount of work required to complete the reaction
    /// </summary>
    public float reactionWorkAmount = 10000f;
}
