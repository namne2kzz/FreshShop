using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshShop.Data.Migrations
{
    public partial class AspNetCoreIdentityDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 504, DateTimeKind.Local).AddTicks(7121),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 16, 39, 6, 92, DateTimeKind.Local).AddTicks(38));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 499, DateTimeKind.Local).AddTicks(1194),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 8, 16, 39, 6, 86, DateTimeKind.Local).AddTicks(5557));

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "AppUser",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 16, 39, 6, 92, DateTimeKind.Local).AddTicks(38),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 504, DateTimeKind.Local).AddTicks(7121));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 16, 39, 6, 86, DateTimeKind.Local).AddTicks(5557),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2022, 9, 8, 16, 41, 40, 499, DateTimeKind.Local).AddTicks(1194));

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "AppUser",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);
        }
    }
}
