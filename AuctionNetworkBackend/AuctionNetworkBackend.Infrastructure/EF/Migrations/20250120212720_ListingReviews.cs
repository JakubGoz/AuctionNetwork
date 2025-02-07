using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionNetworkBackend.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class ListingReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ListingsStatus",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Ended");

            migrationBuilder.UpdateData(
                table: "ListingsStatus",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Sold");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ListingsStatus",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Sold");

            migrationBuilder.UpdateData(
                table: "ListingsStatus",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Ended");
        }
    }
}
