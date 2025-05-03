using Microsoft.EntityFrameworkCore;
using SecureDocumentAudit.DTO;

namespace SecureDocumentService.DataAccess
{
    public class DocumentAuditDbContext : DbContext
    {
        public DocumentAuditDbContext(DbContextOptions<DocumentAuditDbContext> options) : base(options)
        {

        }

        public DbSet<DocumentAuditRequest> DocumentAudits { get; set; } = null!;
       
    }
    
}
