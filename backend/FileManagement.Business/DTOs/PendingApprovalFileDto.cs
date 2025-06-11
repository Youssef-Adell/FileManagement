namespace FileManagement.Business.DTOs
{
    public class PendingApprovalFileDto
    {
        public int Id { get; set; }
        public required string FileNumber { get; set; }
        public required string Subject { get; set; }
        public required string SubmitterName { get; set; }
        public DateTime FileDate { get; set; }
        public required string ClassificationName { get; set; }
        public string? AttachmentUrl { get; set; }
    }
} 