using Microsoft.EntityFrameworkCore.Migrations;

namespace Iep.Migrations
{
    public partial class concurencyToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b5ce9fb-c2af-41d6-aa83-7ed33731891f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e56d871b-6f5f-4c5b-aefd-d6bd73434e1b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "93685703-058b-4dba-94ec-7b20605e0d29", "98a173a5-b70d-4bb3-9ab0-bf5cc64e4e8f", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "309ccc58-9006-489b-80da-d5978ec07bcf", "cdbb4296-bcae-4b59-a66e-e798350a8c78", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "309ccc58-9006-489b-80da-d5978ec07bcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93685703-058b-4dba-94ec-7b20605e0d29");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4b5ce9fb-c2af-41d6-aa83-7ed33731891f", "7c634d3b-0f42-4f7b-a4ce-7196709424c6", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e56d871b-6f5f-4c5b-aefd-d6bd73434e1b", "7477857a-63ef-411f-9c8f-447e63e7e1de", "Administrator", "ADMINISTRATOR" });
        }
    }
}
