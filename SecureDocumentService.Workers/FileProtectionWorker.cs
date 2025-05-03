using System;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SecureDocumentService.Workers;

public class FileProtectionWorker : BackgroundService
    {
        private readonly ILogger<FileProtectionWorker> _logger;

        public FileProtectionWorker(ILogger<FileProtectionWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FileProtectionWorker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                // TODO: Implement file protection logic here.
                _logger.LogInformation("FileProtectionWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("FileProtectionWorker is stopping at: {time}", DateTimeOffset.Now);
        }
    }
