using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApprovalCommentsAndApprovedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "FileApprovals");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "FileApprovals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "FileApprovals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "FileApprovals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
