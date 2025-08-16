using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalog.Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueryHistoryEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QueryTitle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryHistoryEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueryHistoryEntity_CreatedTimestamp",
                table: "QueryHistoryEntity",
                column: "CreatedTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueryHistoryEntity");
        }
    }
}
