using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureDocumentAudit.DTO;
using SecureDocumentService.DataAccess.Interfaces;
using SecureDocumentService.Infrastructure.Interface;

namespace SecureDocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAuditController : ControllerBase
    {
        private readonly IDocumentAuditChannel _documentAuditChannel;
        private readonly IDocumentAuditRepository _documentAuditRepository;

        public DocumentAuditController(IDocumentAuditChannel documentAuditChannel, IDocumentAuditRepository documentAuditRepository)
        {
            // Constructor injection of the document audit channel and repository
            _documentAuditChannel = documentAuditChannel;
            _documentAuditRepository = documentAuditRepository; // Uncomment if you need to use the repository in this controller

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
