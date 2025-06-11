using System.ComponentModel.DataAnnotations;
using FileManagement.Data.Models;

namespace FileManagement.Data.Models
{
    public class FileEntity
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string FileNumber { get; set; }
        
        [Required]
        [MaxLength(200)]
        public required string Subject { get; set; }
        
        public int SubmitterId { get; set; }
        
        public FileStatus Status { get; set; }
        
        public DateTime FileDate { get; set; }
        
        public int ClassificationId { get; set; }
        
        public int ResponsibleEmployeeId { get; set; }
        
        public required string AttachmentUrl { get; set; }
        
        // Navigation properties
        public User? Submitter { get; set; }
        public User? ResponsibleEmployee { get; set; }
        public FileClassification? Classification { get; set; }
        public ICollection<FileApproval> Approvals { get; set; } = new List<FileApproval>();
    }
} 