using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FileManagement.Data.Models;

namespace FileManagement.Data.Models
{
    public class FileApproval
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FileId { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required]
        public int ApprovalOrder { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; }
        
        public FileEntity? File { get; set; }

        public User? Approver { get; set; }
    }
} 