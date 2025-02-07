using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionNetworkBackend.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WinnerId",
                table: "Listings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_WinnerId",
                table: "Listings",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Users_WinnerId",
                table: "Listings",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Users_WinnerId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_WinnerId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Listings");
        }
    }
}
