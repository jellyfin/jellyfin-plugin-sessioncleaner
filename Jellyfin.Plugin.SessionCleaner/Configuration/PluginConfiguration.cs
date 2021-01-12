using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.DeviceCleaner.Configuration
{
    /// <summary>
    /// Plugin configuration.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets the amount of days a device should be kept.
        /// TODO allow user to configure.
        /// </summary>
        public int Days { get; set; } = 20;
    }
}