using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalog.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Update_QueryHistoryEntity_CreatedTimestampIndexDesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueryHistoryEntity_CreatedTimestamp",
                table: "QueryHistoryEntity");

            migrationBuilder.CreateIndex(
                name: "IX_QueryHistoryEntity_CreatedTimestamp",
                table: "QueryHistoryEntity",
                column: "CreatedTimestamp",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QueryHistoryEntity_CreatedTimestamp",
                table: "QueryHistoryEntity");

            migrationBuilder.CreateIndex(
                name: "IX_QueryHistoryEntity_CreatedTimestamp",
                table: "QueryHistoryEntity",
                column: "CreatedTimestamp");
        }
    }
}
