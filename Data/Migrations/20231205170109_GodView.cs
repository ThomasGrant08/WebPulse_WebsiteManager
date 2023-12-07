using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPulse_WebManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class GodView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GlobalAdmin",
                table: "AspNetUsers",
                newName: "GodView");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GodView",
                table: "AspNetUsers",
                newName: "GlobalAdmin");
        }
    }
}
