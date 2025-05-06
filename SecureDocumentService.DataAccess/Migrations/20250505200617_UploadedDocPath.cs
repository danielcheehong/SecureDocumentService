using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureDocumentService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UploadedDocPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentFilePath",
                table: "DocumentAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentFilePath",
                table: "DocumentAudits");
        }
    }
}
