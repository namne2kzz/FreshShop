using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshShop.Data.Migrations
{
    public partial class multilanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 61, DateTimeKind.Local).AddTicks(7393),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 504, DateTimeKind.Local).AddTicks(7121));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 57, DateTimeKind.Local).AddTicks(1728),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 499, DateTimeKind.Local).AddTicks(1194));

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(maxLength: 255, nullable: true),
                    LanguageId = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    SeoAlias = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslation_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoTitle = table.Column<string>(nullable: false),
                    SeoAlias = table.Column<string>(maxLength: 255, nullable: false),
                    LanguageId = table.Column<string>(unicode: false, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTranslation_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslation_CategoryId",
                table: "CategoryTranslation",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslation_LanguageId",
                table: "CategoryTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslation_LanguageId",
                table: "ProductTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslation_ProductId",
                table: "ProductTranslation",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTranslation");

            migrationBuilder.DropTable(
                name: "ProductTranslation");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 504, DateTimeKind.Local).AddTicks(7121),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 61, DateTimeKind.Local).AddTicks(7393));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 499, DateTimeKind.Local).AddTicks(1194),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 57, DateTimeKind.Local).AddTicks(1728));
        }
    }
}
