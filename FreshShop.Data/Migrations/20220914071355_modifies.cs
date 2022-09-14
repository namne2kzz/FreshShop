using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshShop.Data.Migrations
{
    public partial class modifies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Product");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 491, DateTimeKind.Local).AddTicks(4184));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 486, DateTimeKind.Local).AddTicks(6501));

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "9b880e57-2f62-42da-80fa-94d545e2eb0c");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "68964811-fffa-45ce-ba99-eeacc0f690a6", "AQAAAAEAACcQAAAAEL7t9ZZD8iWheT0Gy46vVVGrclGX3FKyAi6p2Zl6qNr9XTRq5oK7dAL78216dmFZ7g==" });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 9, 14, 14, 13, 54, 251, DateTimeKind.Local).AddTicks(1173));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Product",
                type: "nvarchar(255)",
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
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Blog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 9, 8, 21, 40, 16, 486, DateTimeKind.Local).AddTicks(6501),
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "db550f0b-d279-4cdf-aa90-1b784a3d0ddc");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fc97a097-955a-4d31-93ff-2a3a9b274bd6", "AQAAAAEAACcQAAAAEJO0rQMh4ngBEkrBSDuA7ynAkfEFfowfa7dDs7hbQ4NIa0PSGpkpa3xjQFFhoPxGQw==" });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Name" },
                values: new object[] { new DateTime(2022, 9, 8, 21, 40, 16, 533, DateTimeKind.Local).AddTicks(921), "San pham 1" });
        }
    }
}
