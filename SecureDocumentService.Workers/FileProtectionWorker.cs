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

        private void CreateSystemFolders()
        {
            // Create system folders if they do not exist.
            var systemFolders = new[] { "Incoming", "EncryptedDocuments"};
            foreach (var folder in systemFolders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    _logger.LogInformation("Created system folder: {folder}", folder);
                }
                else
                {
                    _logger.LogInformation("System folder already exists: {folder}", folder);
                }
            }
        }

        private void CreateFileProtectionTasks(CancellationToken stoppingToken)
        {
        // Run three instances of ProcessDocumentAuditMessages concurrently, and keep track with _runningTasks.
            for (int i = 0; i < _maxConcurrentTasks; i++)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                
                var taskId = Guid.NewGuid(); // Generate a unique ID for the task
                var workerTask = ProcessDocumentAuditMessages(cts.Token, taskId);
                _runningTasks.TryAdd(taskId, (cts, workerTask)); // Add the task to the running tasks dictionary

                _logger.LogInformation("Started worker task with ID: {taskId}", taskId);
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("FileProtectionWorker started to create file protection tasks: {time}", DateTimeOffset.Now);

            try
            {
                CreateSystemFolders(); // Create system folders at startup
                CreateFileProtectionTasks(stoppingToken); // Start the file protection tasks
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("FileProtectionWorker cancelled before starting tasks.");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error waiting for FileProtectionWorker to be ready: {message}", ex.Message);
                return;
            }
        }

        // Create a method of type task to while loop trying to read the channel and process the messages.
        private async Task ProcessDocumentAuditMessages(CancellationToken cancellationToken, Guid id)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("File Protect Task {id} Waiting for messages to process...", id);
                    var message = await _documentAuditChannel.ReadAsync(cancellationToken);
                    if (message != null)
                    {
                        // Process the message here.
                        // Todo: Implement the logic to process the file calling Rest4CnP.
                        _logger.LogInformation("File Protect Task {id} Processing file: {message}", id, message.DocumentName);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation gracefully.
                    _logger.LogInformation("File Protect Task {id} Processing cancelled.",id);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "File Protect Task {id} Error processing message: {message}", id, ex.Message);
                }
            }
        }
        
    }
