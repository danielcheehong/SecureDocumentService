using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureDocumentService.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewApplicationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                table: "DocumentAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "DocumentAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationSwci",
                table: "DocumentAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "DocumentAudits");

            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "DocumentAudits");

            migrationBuilder.DropColumn(
                name: "ApplicationSwci",
                table: "DocumentAudits");
        }
    }
}
