using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshShop.Data.Migrations
{
    public partial class update_address_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "AddressDetail",
                table: "Address",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Address",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Address",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "f7178b95-9301-4b03-bac4-72f38b95e603");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2fd1744a-e944-4314-ade5-168ddd901219", "AQAAAAEAACcQAAAAEFmTs+uyRJcEteLiI/NexWNfKohxgSG6zwhC30DN+tDZ48wzlwNU9oTrSsuBW+K0Ww==" });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 10, 13, 14, 12, 55, 373, DateTimeKind.Local).AddTicks(6179));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "AddressDetail",
                table: "Address",
                type: "text",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Address",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Address",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "83c583f9-c8ff-46da-817d-af0b6b82f2df");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9bfe85ae-20ae-4617-bca2-70dda434b4d8", "AQAAAAEAACcQAAAAEA1rMNIRJjZxXITj4WU0a9ESzlCLy5ETMx16vlTpnPlcro5mh86EAs9Eh3awj8h8zA==" });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 10, 6, 14, 52, 34, 801, DateTimeKind.Local).AddTicks(6581));
        }
    }
}
