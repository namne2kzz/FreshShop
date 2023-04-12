using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreshShop.Data.Migrations
{
    public partial class modify_tbl_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Order",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "e1d9dee9-d951-4b8b-9b6f-b37e3a450cc1");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fe63d3cd-0c8f-4c33-aea8-9f48deb13832", "AQAAAAEAACcQAAAAEKwqifoHpaZeFjwb4X0xk9ojuGkRoBC+hlotwZbhGDGAFCo8lSpUxEP844ZEVMAo2g==" });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 1, 31, 13, 44, 38, 554, DateTimeKind.Local).AddTicks(3125));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Order");

            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "e1c1c44a-1493-4acc-83f0-65c8d6066b17");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f64583d1-ea82-4539-b352-6a6637b798c0", "AQAAAAEAACcQAAAAEJM5Zc55cfnqy5/t3bfm/F363uX4GU1J6ARcmmE+EEZa2in3VNDCRtvCKAD2LJgInQ==" });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 10, 13, 15, 49, 26, 134, DateTimeKind.Local).AddTicks(6401));
        }
    }
}
