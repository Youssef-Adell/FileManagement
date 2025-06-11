using Microsoft.EntityFrameworkCore;
using FileManagement.Data.Models;

namespace FileManagement.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Seed Users if they don't exist
            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User { Username = "employee1", Password = BCrypt.Net.BCrypt.HashPassword("password123"), FullName = "Youssef Adel", Role = UserRole.Creator },
                    new User { Username = "employee2", Password = BCrypt.Net.BCrypt.HashPassword("password123"), FullName = "Sara Ali", Role = UserRole.Approver },
                    new User { Username = "employee3", Password = BCrypt.Net.BCrypt.HashPassword("password123"), FullName = "Omar Hassan", Role = UserRole.Approver }
                };
                await context.Users.AddRangeAsync(users);
            }

            // Seed Classifications if they don't exist
            if (!await context.FileClassifications.AnyAsync())
            {
                var classifications = new List<FileClassification>
                {
                    new FileClassification { Name = "Administrative" },
                    new FileClassification { Name = "Technical" },
                    new FileClassification { Name = "Financial" },
                    new FileClassification { Name = "Legal" }
                };
                await context.FileClassifications.AddRangeAsync(classifications);
            }

            await context.SaveChangesAsync();
        }
    }
} 