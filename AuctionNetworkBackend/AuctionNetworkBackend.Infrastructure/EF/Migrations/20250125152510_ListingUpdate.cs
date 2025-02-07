using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionNetworkBackend.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class ListingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BuyerId",
                table: "Listings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_BuyerId",
                table: "Listings",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Users_BuyerId",
                table: "Listings",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Users_BuyerId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_BuyerId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Listings");
        }
    }
}
