using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iQuarc.DataLocalization.Tests.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsoCode = table.Column<string>(nullable: true),
                    ThreeLetterIsoCode = table.Column<string>(nullable: true),
                    LCID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryLocalizations",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    CommercialName = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    ShortDescription = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLocalizations", x => new { x.CategoryId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_CategoryLocalizations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryLocalizations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Selection of craft beers", "Beers" },
                    { 2, "Local and international wines", "Wines" },
                    { 3, "Bistro foods and snacks", "Foods" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsoCode", "LCID", "Name", "ThreeLetterIsoCode" },
                values: new object[,]
                {
                    { 1, "fr", 12, "French", "fra" },
                    { 2, "ro", 24, "Romanian", "ron" }
                });

            migrationBuilder.InsertData(
                table: "CategoryLocalizations",
                columns: new[] { "CategoryId", "LanguageId", "CommercialName", "Description", "Name", "ShortDescription" },
                values: new object[,]
                {
                    { 1, 1, null, "Sélection de bières artisanales", "Bières", null },
                    { 2, 1, null, "Vins locaux et internationaux", "Vins", null },
                    { 3, 1, null, "Mets et collations Bistro", "Aliments", null },
                    { 1, 2, null, "Selecţie de bere artizanalã", "Beri", null },
                    { 2, 2, null, "Mâncãruri bistro și gustări", "Vinuri", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLocalizations_LanguageId",
                table: "CategoryLocalizations",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryLocalizations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
