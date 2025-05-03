using System;
using SecureDocumentAudit.DTO;

namespace SecureDocumentService.Infrastructure.Interface;

public interface IDocumentAuditChannel
{
    Task WriteAsync(DocumentAuditRequest request);
    Task<DocumentAuditRequest> ReadAsync(CancellationToken cancellationToken);
}
