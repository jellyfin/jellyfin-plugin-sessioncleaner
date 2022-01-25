using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.SessionCleaner.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the amount of days a device should be kept.
    /// </summary>
    public int Days { get; set; } = 30;
}
