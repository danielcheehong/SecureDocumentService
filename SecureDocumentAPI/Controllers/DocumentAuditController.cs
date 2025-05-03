using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureDocumentAudit.DTO;
using SecureDocumentService.Infrastructure.Interface;

namespace SecureDocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAuditController : ControllerBase
    {
        private readonly IDocumentAuditChannel _documentAuditChannel;

        public DocumentAuditController(IDocumentAuditChannel documentAuditChannel)
        {
            _documentAuditChannel = documentAuditChannel;
        } 

        [HttpPost("audit-document")]  

        public IActionResult RegisterDocumentAudit([FromBody] DocumentAuditRequest request)
        {
            // Logic to register document audit

            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            _documentAuditChannel.WriteAsync(request).Wait(); // Write to the channel

            return Ok(new { Message = "Document audit registered successfully." });
        }
    }
}
