using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestUrls.EntityFramework.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSitemap = table.Column<bool>(type: "bit", nullable: false),
                    IsWeb = table.Column<bool>(type: "bit", nullable: false),
                    InfoEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlEntities_InfoEntities_InfoEntityId",
                        column: x => x.InfoEntityId,
                        principalTable: "InfoEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UrlResponseEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOfResponse = table.Column<int>(type: "int", nullable: false),
                    InfoEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlResponseEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlResponseEntities_InfoEntities_InfoEntityId",
                        column: x => x.InfoEntityId,
                        principalTable: "InfoEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlEntities_InfoEntityId",
                table: "UrlEntities",
                column: "InfoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UrlResponseEntities_InfoEntityId",
                table: "UrlResponseEntities",
                column: "InfoEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlEntities");

            migrationBuilder.DropTable(
                name: "UrlResponseEntities");

            migrationBuilder.DropTable(
                name: "InfoEntities");
        }
    }
}
