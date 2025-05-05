using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecureDocumentService.Infrastructure.Interface;

namespace SecureDocumentService.Workers;

public class FileProtectionWorker : BackgroundService
{
    private readonly int _maxConcurrentTasks = 3; // Maximum number of concurrent tasks
    private readonly ILogger<FileProtectionWorker> _logger;
    private readonly ConcurrentDictionary<Guid, (CancellationTokenSource cts, Task worker)> _runningTasks;
    private readonly IDocumentAuditChannel _documentAuditChannel;

    public FileProtectionWorker(ILogger<FileProtectionWorker> logger, 
                                    ConcurrentDictionary<Guid, (CancellationTokenSource cts, Task worker)> runningTasks,
                                    IDocumentAuditChannel documentAuditChannel)
        {
            _logger = logger;
            _runningTasks = runningTasks; // Inject the running tasks dictionary to allow cancellation of specific tasks.
            _documentAuditChannel = documentAuditChannel; // Inject the DocumentAuditChannel to allow communication with the worker.
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FileProtectionWorker started at: {time}", DateTimeOffset.Now);

           // Run three instances of ProcessDocumentAuditMessages concurrently, and keep track with _runningTasks.
            for (int i = 0; i < _maxConcurrentTasks; i++)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                var workerTask = ProcessDocumentAuditMessages(cts.Token);
                var taskId = Guid.NewGuid(); // Generate a unique ID for the task
                _runningTasks.TryAdd(taskId, (cts, workerTask)); // Add the task to the running tasks dictionary

                _logger.LogInformation("Started worker task with ID: {taskId}", taskId);
            }

            _logger.LogInformation("FileProtectionWorker is stopping at: {time}", DateTimeOffset.Now);
        }

        // Create a method of type task to while loop trying to read the channel and process the messages.
        private async Task ProcessDocumentAuditMessages(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = await _documentAuditChannel.ReadAsync(cancellationToken);
                    if (message != null)
                    {
                        // Process the message here.
                        // Todo: Implement the logic to process the file calling Rest4CnP.
                        _logger.LogInformation("Processing file: {message}", message);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation gracefully.
                    _logger.LogInformation("Processing cancelled.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message: {message}", ex.Message);
                }
            }
        }
                    

        
    }
