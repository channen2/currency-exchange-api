using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRateService.Migrations
{
    /// <inheritdoc />
    public partial class AddTreasuryCurrencyToExchangeRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate_RecordDate",
                table: "ExchangeRates"
            );

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "ExchangeRates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)"
            );

            migrationBuilder.AddColumn<string>(
                name: "TreasuryCurrency",
                table: "ExchangeRates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_TreasuryCurrency_EffectiveDate_RecordDate",
                table: "ExchangeRates",
                columns: new[] { "TreasuryCurrency", "EffectiveDate", "RecordDate" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_TreasuryCurrency_EffectiveDate_RecordDate",
                table: "ExchangeRates"
            );

            migrationBuilder.DropColumn(name: "TreasuryCurrency", table: "ExchangeRates");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "ExchangeRates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate_RecordDate",
                table: "ExchangeRates",
                columns: new[] { "CurrencyCode", "EffectiveDate", "RecordDate" },
                unique: true
            );
        }
    }
}
