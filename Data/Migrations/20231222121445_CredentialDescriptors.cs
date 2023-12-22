using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPulse_WebManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class CredentialDescriptors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Credential",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Credential",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Valid",
                table: "Credential",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Credential");

            migrationBuilder.DropColumn(
                name: "Valid",
                table: "Credential");
        }
    }
}
