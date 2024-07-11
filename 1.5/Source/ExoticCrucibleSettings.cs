using UnityEngine;
using Verse;

namespace ExoticCrucible;

/// <summary>
///     The settings for the mod
/// </summary>
public class ExoticCrucibleSettings : ModSettings
{
    /// <summary>
    ///     The global multiplier applied to the reaction speed
    /// </summary>
    public static float globalReactionSpeedMultiplier = 1f;

    /// <summary>
    ///     The global multiplier applied to the reaction heat bonus
    /// </summary>
    public static float globalReactionHeatBonusMultiplier = 1f;

    /// <summary>
    ///     Expose data to save/load
    /// </summary>
    public override void ExposeData()
    {
        Scribe_Values.Look(ref globalReactionSpeedMultiplier, "globalReactionSpeedMultiplier", 1f);
        Scribe_Values.Look(ref globalReactionHeatBonusMultiplier, "globalReactionHeatBonusMultiplier", 1f);
        base.ExposeData();
    }

    /// <summary>
    ///     Draw the settings window
    /// </summary>
    /// <param name="inRect"></param>
    public static void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        listingStandard.Label("ExoticCrucible.GlobalReactionSpeedMultiplier".Translate() + ": " +
                              globalReactionSpeedMultiplier);
        globalReactionSpeedMultiplier = listingStandard.Slider(globalReactionSpeedMultiplier, 0.1f, 10f);

        listingStandard.Label("ExoticCrucible.GlobalReactionHeatBonusMultiplier".Translate() + ": " +
                              globalReactionHeatBonusMultiplier);
        globalReactionHeatBonusMultiplier = listingStandard.Slider(globalReactionHeatBonusMultiplier, 0.1f, 10f);

        listingStandard.Gap();

        if (listingStandard.ButtonText("ExoticCrucible.ResetSettings".Translate())) ResetSettings();

        listingStandard.End();
    }

    /// <summary>
    ///     Reset the settings to default
    /// </summary>
    private static void ResetSettings()
    {
        globalReactionSpeedMultiplier = 1f;
        globalReactionHeatBonusMultiplier = 1f;
    }
}
