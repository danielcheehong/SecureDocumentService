namespace SecureDocumentAudit.DTO
{
    public class DocumentAuditRequest
    {
        public string UserId { get; set; }
        public string DocumentId { get; set; }
        public string DocumentType { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
    }

}
