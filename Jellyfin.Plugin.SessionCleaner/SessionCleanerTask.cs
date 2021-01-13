using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Security;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.Tasks;

namespace Jellyfin.Plugin.DeviceCleaner
{
    /// <summary>
    /// Device cleaner task.
    /// </summary>
    public class SessionCleanerTask : IScheduledTask, IConfigurableScheduledTask
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ISessionManager _sessionManager;
        private readonly ILocalizationManager _localizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCleanerTask"/> class.
        /// </summary>
        /// <param name="authenticationRepository">Instance of the <see cref="IAuthenticationRepository"/> interface.</param>
        /// <param name="sessionManager">Instance of the <see cref="ISessionManager"/> interface.</param>
        /// <param name="localizationManager">Instance of the <see cref="ILocalizationManager"/> interface.</param>
        public SessionCleanerTask(
            IAuthenticationRepository authenticationRepository,
            ISessionManager sessionManager,
            ILocalizationManager localizationManager)
        {
            _authenticationRepository = authenticationRepository;
            _sessionManager = sessionManager;
            _localizationManager = localizationManager;
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
        public Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            var expireDays = SessionCleanerPlugin.Instance?.Configuration.Days
                             ?? throw new Exception("Plugin instance is null");
            var expireDate = DateTime.UtcNow.AddDays(expireDays * -1);
            var sessions = _authenticationRepository.Get(new AuthenticationInfoQuery
            {
                HasUser = true
            })?.Items;

            if (sessions == null)
            {
                return Task.CompletedTask;
            }

            foreach (var session in sessions)
            {
                if (session.DateLastActivity < expireDate)
                {
                    _sessionManager.Logout(session);
                }
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return Enumerable.Empty<TaskTriggerInfo>();
        }
    }
}