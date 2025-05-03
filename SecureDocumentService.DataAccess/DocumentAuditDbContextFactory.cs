using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SecureDocumentService.DataAccess;

public class DocumentAuditDbContextFactory:IDesignTimeDbContextFactory<DocumentAuditDbContext>
{
    public DocumentAuditDbContext CreateDbContext(string[] args = null!)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DocumentAuditDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=DocumentAudit;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;");

        return new DocumentAuditDbContext(optionsBuilder.Options);
    }
}
