using UnityEngine;
using Verse;

namespace ExoticCrucible;

/// <summary>
/// The main class for the ExoticCrucible mod
/// </summary>
public class ExoticCrucible : Mod
{
    /// <summary>
    /// The settings for the mod
    /// </summary>
    public ExoticCrucibleSettings settings;

    /// <summary>
    /// Constructor for the mod class to get the settings
    /// </summary>
    /// <param name="content"></param>
    public ExoticCrucible(ModContentPack content) : base(content)
    {
        settings = GetSettings<ExoticCrucibleSettings>();
    }

    /// <summary>
    /// Draw the settings window
    /// </summary>
    /// <param name="inRect"></param>
    public override void DoSettingsWindowContents(Rect inRect)
    {
        settings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }

    /// <summary>
    /// Add the settings category
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory() => "ExoticCrucible".Translate();
}