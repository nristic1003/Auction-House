using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iep.Migrations
{
    public partial class DrugaMigracija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0533e3b4-392e-445b-b098-1d8aeb1dd486");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e2cbb47-fbf9-4a82-bc31-43c49d0ac2ba");

            migrationBuilder.RenameColumn(
                name: "Tokens",
                table: "AspNetUsers",
                newName: "tokens");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "AspNetUsers",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "AspNetUsers",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "firstName");

            migrationBuilder.AlterColumn<int>(
                name: "tokens",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "AspNetUsers",
                type: "varchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldMaxLength: 1);

            migrationBuilder.CreateTable(
                name: "auction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 20, nullable: false),
                    description = table.Column<string>(nullable: false),
                    image = table.Column<byte[]>(nullable: false),
                    startPrice = table.Column<int>(nullable: false),
                    currentPrice = table.Column<int>(nullable: false),
                    createDate = table.Column<DateTime>(nullable: false),
                    openDate = table.Column<DateTime>(nullable: false),
                    closeDate = table.Column<DateTime>(nullable: false),
                    state = table.Column<string>(nullable: false),
                    winnerId = table.Column<string>(nullable: true),
                    ownerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_auction_AspNetUsers_ownerId",
                        column: x => x.ownerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_auction_AspNetUsers_winnerId",
                        column: x => x.winnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    idT = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(nullable: false),
                    idUser = table.Column<string>(nullable: false),
                    idToken = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.idT);
                    table.ForeignKey(
                        name: "FK_transaction_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transaction_tokens_idToken",
                        column: x => x.idToken,
                        principalTable: "tokens",
                        principalColumn: "idToken",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bids",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    auctionId = table.Column<int>(nullable: false),
                    price = table.Column<int>(nullable: false),
                    bidDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bids", x => new { x.userId, x.auctionId });
                    table.ForeignKey(
                        name: "FK_bids_auction_auctionId",
                        column: x => x.auctionId,
                        principalTable: "auction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bids_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4b481a26-8ca4-4fea-820b-0b046e75ee18", "df8e7ff8-3c75-44af-97cc-a079aef04889", "User", "USER" },
                    { "afaf35cc-37d6-49ce-abb7-318b72796fba", "2e8dabd3-5dc3-4515-b164-b21652cf114c", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "tokens",
                columns: new[] { "idToken", "amount", "name", "price" },
                values: new object[,]
                {
                    { 1, 5, "Silver", 7 },
                    { 2, 10, "Gold", 12 },
                    { 3, 20, "Platinum", 19 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_auction_ownerId",
                table: "auction",
                column: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_auction_winnerId",
                table: "auction",
                column: "winnerId");

            migrationBuilder.CreateIndex(
                name: "IX_bids_auctionId",
                table: "bids",
                column: "auctionId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_UserId",
                table: "transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_idToken",
                table: "transaction",
                column: "idToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bids");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "auction");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b481a26-8ca4-4fea-820b-0b046e75ee18");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afaf35cc-37d6-49ce-abb7-318b72796fba");

            migrationBuilder.DeleteData(
                table: "tokens",
                keyColumn: "idToken",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tokens",
                keyColumn: "idToken",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tokens",
                keyColumn: "idToken",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "tokens",
                table: "AspNetUsers",
                newName: "Tokens");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "AspNetUsers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "AspNetUsers",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "AspNetUsers",
                newName: "FirstName");

            migrationBuilder.AlterColumn<int>(
                name: "Tokens",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "varchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e2cbb47-fbf9-4a82-bc31-43c49d0ac2ba", "6dcf9ab0-f219-4815-adb9-f1adf3310a6c", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0533e3b4-392e-445b-b098-1d8aeb1dd486", "26aadf1c-8ac6-4ddc-a9ed-14422062e66d", "Administrator", "ADMINISTRATOR" });
        }
    }
}
