using System;
using Microsoft.EntityFrameworkCore;
using SecureDocumentAudit.DTO;
using SecureDocumentService.DataAccess.Interfaces;

namespace SecureDocumentService.DataAccess.Repositories;

public class DocumentAuditRepository : IDocumentAuditRepository
{
    private DocumentAuditDbContext _dbDocAuditContext;

    public DocumentAuditRepository(DocumentAuditDbContext dbAuditContext)
    {
        _dbDocAuditContext = dbAuditContext;
    }
   
    public Task AddAsync(DocumentAuditRequest documentAuditRequest, CancellationToken cancellationToken = default)
    {
        _dbDocAuditContext.DocumentAudits.Add(documentAuditRequest);
        return _dbDocAuditContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _dbDocAuditContext.DocumentAudits.Remove(new DocumentAuditRequest { Id = id });
        return _dbDocAuditContext.SaveChangesAsync(cancellationToken);
    }

    public Task<IEnumerable<DocumentAuditRequest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _dbDocAuditContext.DocumentAudits.ToListAsync(cancellationToken);
        return _dbDocAuditContext.DocumentAudits.ToListAsync(cancellationToken).ContinueWith(task => (IEnumerable<DocumentAuditRequest>)task.Result, cancellationToken);
    }

    public Task<DocumentAuditRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       throw new NotImplementedException("GetByIdAsync method is not implemented yet.");
    }

    public Task UpdateAsync(DocumentAuditRequest documentAuditRequest, CancellationToken cancellationToken = default)
    {
       _dbDocAuditContext.DocumentAudits.Update(documentAuditRequest);
        return _dbDocAuditContext.SaveChangesAsync(cancellationToken);
    }
}
