using UnityEngine;
using Verse;

namespace ExoticHeatsink;

/// <summary>
/// The main class for the ExoticHeatsink mod
/// </summary>
public class ExoticHeatsink : Mod
{
    /// <summary>
    /// The settings for the mod
    /// </summary>
    public ExoticHeatsinkSettings settings;

    /// <summary>
    /// Constructor for the mod class to get the settings
    /// </summary>
    /// <param name="content"></param>
    public ExoticHeatsink(ModContentPack content) : base(content)
    {
        settings = GetSettings<ExoticHeatsinkSettings>();
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
    public override string SettingsCategory() => "ExoticHeatsink".Translate();
}