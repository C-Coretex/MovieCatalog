using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalog.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Update_QueryHistoryEntity_TableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QueryHistoryEntity",
                table: "QueryHistoryEntity");

            migrationBuilder.RenameTable(
                name: "QueryHistoryEntity",
                newName: "QueryHistory");

            migrationBuilder.RenameIndex(
                name: "IX_QueryHistoryEntity_CreatedTimestamp",
                table: "QueryHistory",
                newName: "IX_QueryHistory_CreatedTimestamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueryHistory",
                table: "QueryHistory",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QueryHistory",
                table: "QueryHistory");

            migrationBuilder.RenameTable(
                name: "QueryHistory",
                newName: "QueryHistoryEntity");

            migrationBuilder.RenameIndex(
                name: "IX_QueryHistory_CreatedTimestamp",
                table: "QueryHistoryEntity",
                newName: "IX_QueryHistoryEntity_CreatedTimestamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueryHistoryEntity",
                table: "QueryHistoryEntity",
                column: "Id");
        }
    }
}
