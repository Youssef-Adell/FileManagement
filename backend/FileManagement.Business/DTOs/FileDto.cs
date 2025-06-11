namespace FileManagement.Business.DTOs
{
    public class FileDto
    {
        public int Id { get; set; }
        public required string FileNumber { get; set; }
        public required string Subject { get; set; }
        public required string SubmitterName { get; set; }
        public required string Status { get; set; }
        public DateTime FileDate { get; set; }
        public required string ClassificationName { get; set; }
        public required string ResponsibleEmployeeName { get; set; }
        public string? AttachmentUrl { get; set; }
    }
} 