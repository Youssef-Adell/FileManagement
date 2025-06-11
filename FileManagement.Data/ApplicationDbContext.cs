using Microsoft.EntityFrameworkCore;
using FileManagement.Data.Models;

namespace FileManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<FileClassification> FileClassifications { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<FileApproval> FileApprovals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // FileEntity configurations
            modelBuilder.Entity<FileEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.FileNumber).IsUnique();
                
                // Enum conversions
                entity.Property(e => e.Status)
                    .HasConversion(
                        status => status.ToString(),
                        status => Enum.Parse<FileStatus>(status));
                
                entity.HasOne(f => f.Submitter)
                    .WithMany(u => u.SubmittedFiles)
                    .HasForeignKey(f => f.SubmitterId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(f => f.ResponsibleEmployee)
                    .WithMany(u => u.ResponsibleFiles)
                    .HasForeignKey(f => f.ResponsibleEmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(f => f.Classification)
                    .WithMany(c => c.Files)
                    .HasForeignKey(f => f.ClassificationId);
            });

            // FileApproval configurations
            modelBuilder.Entity<FileApproval>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Enum conversions
                entity.Property(e => e.Status)
                    .HasConversion(
                        status => status.ToString(),
                        status => Enum.Parse<ApprovalStatus>(status));
                
                entity.HasOne(a => a.File)
                    .WithMany(f => f.Approvals)
                    .HasForeignKey(a => a.FileId);
                
                entity.HasOne(a => a.Approver)
                    .WithMany(u => u.FileApprovals)
                    .HasForeignKey(a => a.ApproverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 