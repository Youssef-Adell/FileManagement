using System.ComponentModel.DataAnnotations;

namespace FileManagement.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }
        
        [Required]
        public required string Password { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string FullName { get; set; }

        [Required]
        public UserRole Role { get; set; }
        
        // Navigation properties
        public ICollection<FileEntity> SubmittedFiles { get; set; } = new List<FileEntity>();
        public ICollection<FileEntity> ResponsibleFiles { get; set; } = new List<FileEntity>();
        public ICollection<FileApproval> FileApprovals { get; set; } = new List<FileApproval>();
    }
} 