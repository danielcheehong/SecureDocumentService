using System;
using System.Threading.Channels;
using SecureDocumentAudit.DTO;
using SecureDocumentService.Infrastructure.Interface;

namespace SecureDocumentService.Infrastructure.Implementation;

public class DocumentAuditChannel: IDocumentAuditChannel
{
    private readonly Channel<DocumentAuditRequest> _channel;
    public DocumentAuditChannel()
    {
        // Consider making the channel capacity configurable via app settings or environment variables.
        _channel = Channel.CreateBounded<DocumentAuditRequest>(new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true, // Let´s assume only the worker will be only one reading from the channel.
            SingleWriter = true // Let´s assume only the API will write to the channel.
        });   
        
    }

    public async Task WriteAsync(DocumentAuditRequest request)
    {
        await _channel.Writer.WriteAsync(request);
    }
    
    public async Task<DocumentAuditRequest> ReadAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }    

}