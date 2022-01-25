using System;
using System.Collections.Generic;
using Jellyfin.Plugin.SessionCleaner.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.SessionCleaner;

/// <summary>
/// Plugin entrypoint.
/// </summary>
public class SessionCleanerPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly Guid _id = new("EC9E2A74-1311-4A14-B302-158E3D95FD1D");

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionCleanerPlugin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
    public SessionCleanerPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static SessionCleanerPlugin? Instance { get; private set; }

    /// <inheritdoc />
    public override Guid Id => _id;

    /// <inheritdoc />
    public override string Name => "Session Cleaner";

    /// <inheritdoc />
    public override string Description => "Cleans old sessions.";

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = Name,
            EmbeddedResourcePath = GetType().Namespace + ".Configuration.config.html"
        };
    }
}
