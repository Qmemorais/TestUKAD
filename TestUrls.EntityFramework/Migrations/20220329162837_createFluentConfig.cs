using Microsoft.EntityFrameworkCore.Migrations;

namespace TestUrls.EntityFramework.Migrations
{
    public partial class createFluentConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlEntities");

            migrationBuilder.DropTable(
                name: "UrlResponseEntities");

            migrationBuilder.CreateTable(
                name: "UrlWithResponseEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSitemap = table.Column<bool>(type: "bit", nullable: false),
                    IsWeb = table.Column<bool>(type: "bit", nullable: false),
                    TimeOfResponse = table.Column<int>(type: "int", nullable: false),
                    TestEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlWithResponseEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlWithResponseEntities_InfoEntities_TestEntityId",
                        column: x => x.TestEntityId,
                        principalTable: "InfoEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlWithResponseEntities_TestEntityId",
                table: "UrlWithResponseEntities",
                column: "TestEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlWithResponseEntities");

            migrationBuilder.CreateTable(
                name: "UrlEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoEntityId = table.Column<int>(type: "int", nullable: false),
                    IsSitemap = table.Column<bool>(type: "bit", nullable: false),
                    IsWeb = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    InfoEntityId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOfResponse = table.Column<int>(type: "int", nullable: false)
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
    }
}
