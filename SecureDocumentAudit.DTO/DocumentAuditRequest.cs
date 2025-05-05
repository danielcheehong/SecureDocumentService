namespace SecureDocumentAudit.DTO
{
    public class DocumentAuditRequest
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string DocumentName { get; set; } = null!;
        public string DocumentId { get; set; }= null!;

        public string DocumentType { get; set; }= null!;
        public string ApplicationId { get; set; }= null!;
        public string ApplicationName { get; set; }= null!;
        public string ApplicationSwci { get; set; }= null!;

        public DateTime RequestDate { get; set; }

        public string Description { get; set; } = "";
    }

}
