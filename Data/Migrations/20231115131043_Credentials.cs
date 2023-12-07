using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPulse_WebManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Credentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserWebsite",
                columns: table => new
                {
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WebsitesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserWebsite", x => new { x.UsersId, x.WebsitesId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserWebsite_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserWebsite_Website_WebsitesId",
                        column: x => x.WebsitesId,
                        principalTable: "Website",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebsiteId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credential_Website_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Website",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserCredential",
                columns: table => new
                {
                    AssignedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CredentialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCredential", x => new { x.AssignedUsersId, x.CredentialsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCredential_AspNetUsers_AssignedUsersId",
                        column: x => x.AssignedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCredential_Credential_CredentialsId",
                        column: x => x.CredentialsId,
                        principalTable: "Credential",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCredential_CredentialsId",
                table: "ApplicationUserCredential",
                column: "CredentialsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserWebsite_WebsitesId",
                table: "ApplicationUserWebsite",
                column: "WebsitesId");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_WebsiteId",
                table: "Credential",
                column: "WebsiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCredential");

            migrationBuilder.DropTable(
                name: "ApplicationUserWebsite");

            migrationBuilder.DropTable(
                name: "Credential");
        }
    }
}
