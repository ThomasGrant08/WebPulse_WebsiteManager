using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPulse_WebManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class UsernameSuffix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usernameSuffix",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usernameSuffix",
                table: "AspNetUsers");
        }
    }
}
