using System.Threading.Tasks;
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
        public async Task<IActionResult> RegisterDocumentAudit([FromBody] DocumentAuditRequest request)
        {
            // Logic to register document audit

            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            await _documentAuditRepository.AddAsync(request); // Save to the database (if needed)

            await _documentAuditChannel.WriteAsync(request); // Write to the channel

            return Ok(new { Message = "Document audit registered successfully." });
        }

        // [HttpPost("encrypt-document")]
        // [Consumes("multipart/form-data")]
        // [RequestSizeLimit(150_000_000)] // 150 MB limit
        // [RequestFormLimits(MultipartBodyLengthLimit = 150_000_000)] // 150 MB limit
        // public async Task<IActionResult> EncryptDocument([FromForm] IFormFile file, [FromQuery] string documentId)
        // {
        //     if (file == null || file.Length == 0)
        //         return BadRequest("No file provided.");

        //     // azure_development-get_best_practices
        //     var uploadsDir =  "Incoming";
        //     Directory.CreateDirectory(uploadsDir);

        //     var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //     var filePath   = Path.Combine(uploadsDir, uniqueName);

        //     await using var fs = new FileStream(filePath, FileMode.Create);
        //     await file.CopyToAsync(fs);

        //     var relativeUrl = $"/Uploads/{uniqueName}";
        //     return Ok(new { originalName = file.FileName, storedName = uniqueName, url = relativeUrl });
        // }
        
    }
}
