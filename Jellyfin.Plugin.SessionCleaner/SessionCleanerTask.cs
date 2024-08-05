using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Queries;
using MediaBrowser.Controller.Devices;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.Tasks;

namespace Jellyfin.Plugin.SessionCleaner;

/// <summary>
/// Device cleaner task.
/// </summary>
public class SessionCleanerTask : IScheduledTask, IConfigurableScheduledTask
{
    private readonly IDeviceManager _deviceManager;
    private readonly ISessionManager _sessionManager;
    private readonly ILocalizationManager _localizationManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionCleanerTask"/> class.
    /// </summary>
    /// <param name="sessionManager">Instance of the <see cref="ISessionManager"/> interface.</param>
    /// <param name="localizationManager">Instance of the <see cref="ILocalizationManager"/> interface.</param>
    /// <param name="deviceManager">Instance of the <see cref="IDeviceManager"/> interface.</param>
    public SessionCleanerTask(
        ISessionManager sessionManager,
        ILocalizationManager localizationManager,
        IDeviceManager deviceManager)
    {
        _sessionManager = sessionManager;
        _localizationManager = localizationManager;
        _deviceManager = deviceManager;
    }

    /// <inheritdoc />
    public bool IsHidden => false;

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public bool IsLogged => true;

    /// <inheritdoc />
    public string Name => "Clean Old Sessions";

    /// <inheritdoc />
    public string Key => "CleanOldSessions";

    /// <inheritdoc />
    public string Description => "Removes sessions older then the configured age.";

    /// <inheritdoc />
    public string Category => _localizationManager.GetLocalizedString("TasksMaintenanceCategory");

    /// <inheritdoc />
    public async Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(SessionCleanerPlugin.Instance?.Configuration);
        var expireDays = SessionCleanerPlugin.Instance.Configuration.Days;
        var expireDate = DateTime.UtcNow.AddDays(expireDays * -1);
        var deviceResult = _deviceManager.GetDevices(new DeviceQuery());
        var devices = deviceResult?.Items;

        if (devices is null)
        {
            return;
        }

        foreach (var device in devices)
        {
            if (device.DateLastActivity < expireDate)
            {
                await _sessionManager.Logout(device).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc />
    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        return Enumerable.Empty<TaskTriggerInfo>();
    }
}
