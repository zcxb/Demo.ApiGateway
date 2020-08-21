using Microsoft.Extensions.Hosting;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Ocelot.Configuration
{
    public class DbConfigurationPoller : IHostedService, IDisposable
    {
        private readonly IOcelotLogger _logger;
        private readonly OcelotDbConfiguration _option;

        private Timer _timer;
        private bool _polling;

        private readonly IFileConfigurationRepository _fileRepository;
        private readonly IInternalConfigurationRepository _internalRepository;
        private readonly IInternalConfigurationCreator _internalCreator;

        public DbConfigurationPoller(IOcelotLoggerFactory loggerFactory,
                                     OcelotDbConfiguration option,
                                     IFileConfigurationRepository fileConfiguration,
                                     IInternalConfigurationRepository internalRepository,
                                     IInternalConfigurationCreator internalCreator)
        {
            _logger = loggerFactory.CreateLogger<DbConfigurationPoller>();
            _option = option;
            _fileRepository = fileConfiguration;
            _internalRepository = internalRepository;
            _internalCreator = internalCreator;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_option.EnableTimer)
            {
                _logger.LogInformation($"{nameof(DbConfigurationPoller)} is starting.");
                _timer = new Timer(async x =>
                {
                    if (_polling)
                    {
                        return;
                    }
                    _polling = true;
                    await Poll();
                    _polling = false;
                }, null, _option.TimerDelay, _option.TimerDelay);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_option.EnableTimer)
            {
                _logger.LogInformation($"{nameof(DbConfigurationPoller)} is stopping.");
                _timer?.Change(Timeout.Infinite, 0);
            }
            return Task.CompletedTask;
        }

        private async Task Poll()
        {
            _logger.LogInformation("Polling started.");

            var fileConfig = await _fileRepository.Get();
            if (fileConfig.IsError)
            {
                _logger.LogWarning($"Error getting file configs, errors are [{string.Join(",", fileConfig.Errors.Select(x => x.Message))}].");
                return;
            }
            else
            {
                var config = await _internalCreator.Create(fileConfig.Data);
                if (!config.IsError)
                {
                    _internalRepository.AddOrReplace(config.Data);
                }
            }
            _logger.LogInformation("Polling finished.");
        }
    }
}
