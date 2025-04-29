using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureDocumentAudit.DTO;

namespace SecureDocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAuditController : ControllerBase
    {
        public IActionResult RegisterDocumentAudit([FromBody] DocumentAuditRequest request)
        {
            // Logic to register document audit
            return Ok(new { Message = "Document audit registered successfully." });
        }
    }
}
