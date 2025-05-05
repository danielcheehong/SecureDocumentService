using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureDocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceControlController : ControllerBase
    {
        private ConcurrentDictionary<Guid, (CancellationTokenSource, Task)> _runningTasks;

        public ServiceControlController(ConcurrentDictionary<Guid, (CancellationTokenSource, Task)> runningTasks)
        {
            _runningTasks = runningTasks; // Inject the running tasks dictionary to allow 
        }

        // GET api/servicecontrol/status
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var status = new List<object>();
            foreach (var entry in _runningTasks)
            {
                status.Add(new
                {
                    Id = entry.Key,
                    Status = entry.Value.Item2.Status.ToString(), // Show the current status of the task
                    CancellationRequested = entry.Value.Item1.IsCancellationRequested // Check if cancellation has been requested
                });
            }

            return Ok(status);
        }

         // POST api/servicecontrol/cancel/{id}
        [HttpPost("cancel/{id}")]
        public IActionResult Cancel(Guid id)
        {
            if (_runningTasks.TryGetValue(id, out var entry))
            {
                // Signal cancellation of specific task.
                entry.Item1.Cancel();
                // Optionally, you can remove the task from the dictionary if you want to clean up after cancellation.
                _runningTasks.TryRemove(id, out _);
                return Ok(new { Message = $"Cancellation requested for {id}" });
            }

            return NotFound($"No running task found with id={id}");
        }

    }
}
