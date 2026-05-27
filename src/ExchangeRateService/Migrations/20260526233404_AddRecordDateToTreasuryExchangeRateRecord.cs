using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRateService.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordDateToTreasuryExchangeRateRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate",
                table: "ExchangeRates"
            );

            migrationBuilder.DropColumn(name: "TreasuryCurrencyName", table: "ExchangeRates");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordDate",
                table: "ExchangeRates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
            );

            migrationBuilder.CreateTable(
                name: "IngestionRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionRuns", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate_RecordDate",
                table: "ExchangeRates",
                columns: new[] { "CurrencyCode", "EffectiveDate", "RecordDate" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "IngestionRuns");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate_RecordDate",
                table: "ExchangeRates"
            );

            migrationBuilder.DropColumn(name: "RecordDate", table: "ExchangeRates");

            migrationBuilder.AddColumn<string>(
                name: "TreasuryCurrencyName",
                table: "ExchangeRates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate",
                table: "ExchangeRates",
                columns: new[] { "CurrencyCode", "EffectiveDate" },
                unique: true
            );
        }
    }
}
