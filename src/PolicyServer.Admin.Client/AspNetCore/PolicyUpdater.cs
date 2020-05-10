using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PolicyServer.Admin.Client.AspNetCore
{
    internal class PolicyUpdater
    {
        private readonly ILogger<PolicyUpdater> _logger;
        private readonly IServiceProvider _serviceProvider;
        //private readonly OperationalStoreOptions _options;

        private CancellationTokenSource _source;

        public TimeSpan CleanupInterval => TimeSpan.FromSeconds(30);

        public PolicyUpdater(IServiceProvider serviceProvider, ILogger<PolicyUpdater> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Start()
        {
            Start(CancellationToken.None);
        }

        public void Start(CancellationToken cancellationToken)
        {
            if (_source != null) throw new InvalidOperationException("Already started. Call Stop first.");

            _logger.LogDebug("Starting policy updater");

            _source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Factory.StartNew(() => StartInternal(_source.Token));
        }

        public void Stop()
        {
            if (_source == null) throw new InvalidOperationException("Not started. Call Start first.");

            _logger.LogDebug("Stopping policy updater");

            _source.Cancel();
            _source = null;
        }

        private async Task StartInternal(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested. Exiting.");
                    break;
                }

                try
                {
                    await Task.Delay(CleanupInterval, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogDebug("TaskCanceledException. Exiting.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Task.Delay exception: {0}. Exiting.", ex.Message);
                    break;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("CancellationRequested. Exiting.");
                    break;
                }

                CheckPolicyUpdates();
            }
        }

        public void CheckPolicyUpdates()
        {
            try
            {
                _logger.LogTrace("Asking PolicyServer for any updates");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception checking policy update {exception}", ex.Message);
            }
        }
    }
}
