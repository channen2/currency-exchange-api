using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRateService.Migrations
{
    /// <inheritdoc />
    public partial class AddExchangeRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TreasuryCurrencyName = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false
                    ),
                    Rate = table.Column<decimal>(
                        type: "decimal(18,6)",
                        precision: 18,
                        scale: 6,
                        nullable: false
                    ),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RetrievedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyCode_EffectiveDate",
                table: "ExchangeRates",
                columns: new[] { "CurrencyCode", "EffectiveDate" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ExchangeRates");
        }
    }
}
