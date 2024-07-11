using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Verse;

namespace ExoticCrucible;

/// <summary>
///     The main class for the ExoticCrucible mod
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ExoticCrucible : Mod
{
    /// <summary>
    ///     The settings for the mod
    /// </summary>
    public readonly ExoticCrucibleSettings Settings;

    /// <summary>
    ///     Constructor for the mod class to get the settings
    /// </summary>
    /// <param name="content"></param>
    public ExoticCrucible(ModContentPack content) : base(content)
    {
        Settings = GetSettings<ExoticCrucibleSettings>();
    }

    /// <summary>
    ///     Draw the settings window
    /// </summary>
    /// <param name="inRect"></param>
    public override void DoSettingsWindowContents(Rect inRect)
    {
        ExoticCrucibleSettings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }

    /// <summary>
    ///     Add the settings category
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "ExoticCrucible".Translate();
    }
}
