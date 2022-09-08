using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshShop.Data.Migrations
{
    public partial class seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "ProductTranslation");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CategoryTranslation");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "SeoTitle",
                table: "ProductTranslation",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SeoAlias",
                table: "ProductTranslation",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Product",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Image",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "SeoTitle",
                table: "CategoryTranslation",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoAlias",
                table: "CategoryTranslation",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Category",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 491, DateTimeKind.Local).AddTicks(4184),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 61, DateTimeKind.Local).AddTicks(7393));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 486, DateTimeKind.Local).AddTicks(6501),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 57, DateTimeKind.Local).AddTicks(1728));

            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "db550f0b-d279-4cdf-aa90-1b784a3d0ddc", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "Firstname", "ImagePath", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), 0, "fc97a097-955a-4d31-93ff-2a3a9b274bd6", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tedu.international@gmail.com", true, "Nam", "avatar.jpg", "Huynh Phuong", false, null, "tedu.international@gmail.com", "admin", "AQAAAAEAACcQAAAAEJO0rQMh4ngBEkrBSDuA7ynAkfEFfowfa7dDs7hbQ4NIa0PSGpkpa3xjQFFhoPxGQw==", null, false, "", false, "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new Guid("8d04dce2-969a-435d-bba4-df3f325983dc") });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "ID", "ImagePath", "IsShowOnHome", "Name", "ParentID" },
                values: new object[,]
                {
                    { 1, "cate1.jpg", true, "Loai 1", null },
                    { 2, "cate2.jpg", true, "Loai 2", null }
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "IsDefault", "Name" },
                values: new object[,]
                {
                    { "vi", true, "Tiếng Việt" },
                    { "en", false, "English" }
                });

            migrationBuilder.InsertData(
                table: "CategoryTranslation",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SeoAlias", "SeoTitle" },
                values: new object[,]
                {
                    { 1, 1, "vi", "Loai 1", "loai-1", "San pham cho loai 1 ne" },
                    { 3, 2, "vi", "Loai 2", "loai-2", "San pham cho loai 2 ne" },
                    { 2, 1, "en", "Category 1", "category-1", "Products for category 1" },
                    { 4, 2, "en", "Category 2", "category-2", "Products for category 2" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ID", "CategoryID", "CreatedDate", "Name", "OriginalPrice", "Price", "Sold", "Status", "Stock", "Unit", "ViewCount" },
                values: new object[] { 1, 1, new DateTime(2022, 9, 8, 21, 40, 16, 533, DateTimeKind.Local).AddTicks(921), "San pham 1", 8000m, 10000m, 0, true, 10, "kg", 0 });

            migrationBuilder.InsertData(
                table: "ProductTranslation",
                columns: new[] { "Id", "Description", "LanguageId", "Name", "ProductId", "SeoAlias", "SeoTitle" },
                values: new object[] { 1, "San pham 1", "vi", "San pham 1", 1, "san-pham-1", "San pham 1" });

            migrationBuilder.InsertData(
                table: "ProductTranslation",
                columns: new[] { "Id", "Description", "LanguageId", "Name", "ProductId", "SeoAlias", "SeoTitle" },
                values: new object[] { 2, "Product 2", "en", "Product 2", 1, "product-2", "Product 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"));

            migrationBuilder.DeleteData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new Guid("8d04dce2-969a-435d-bba4-df3f325983dc") });

            migrationBuilder.DeleteData(
                table: "CategoryTranslation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTranslation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoryTranslation",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoryTranslation",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductTranslation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductTranslation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "Id",
                keyValue: "en");

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "Id",
                keyValue: "vi");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "SeoTitle",
                table: "ProductTranslation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "SeoAlias",
                table: "ProductTranslation",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "ProductTranslation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Image",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "SeoTitle",
                table: "CategoryTranslation",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoAlias",
                table: "CategoryTranslation",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CategoryTranslation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Category",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 61, DateTimeKind.Local).AddTicks(7393),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 491, DateTimeKind.Local).AddTicks(4184));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 20, 33, 3, 57, DateTimeKind.Local).AddTicks(1728),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 486, DateTimeKind.Local).AddTicks(6501));
        }
    }
}
