using System;
using SecureDocumentAudit.DTO;

namespace SecureDocumentService.DataAccess.Interfaces;

public interface IDocumentAuditRepository
{
    Task<DocumentAuditRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentAuditRequest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DocumentAuditRequest documentAuditRequest, CancellationToken cancellationToken = default);
    Task UpdateAsync(DocumentAuditRequest documentAuditRequest, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

}
