using System.Net;
using System.Text;
using ExchangeRateService.Common.Errors;
using ExchangeRateService.Services;
using ExchangeRateService.Tests.Common.Http;

namespace ExchangeRateService.Tests.Services
{
    public class TreasuryExchangeRateApiClientTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnSuccess_WhenApiReturnsValidResponse()
        {
            // Arrange
            var json = /*lang=json,strict*/
                """
                {
                    "data": [
                        {
                            "country_currency_desc": "Canada-Dollar",
                            "exchange_rate": "1.25",
                            "record_date": "2026-01-01"
                        }
                    ]
                }
                """;

            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            });

            var httpClient = new HttpClient(handler);

            var service = new TreasuryExchangeRateApiClient(httpClient);

            // Act
            var result = await service.GetExchangeRatesAsync(
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 31)
            );

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Data);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnFailure_WhenApiReturnsNonSuccessStatus()
        {
            // Arrange
            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(
                HttpStatusCode.InternalServerError
            ));

            var httpClient = new HttpClient(handler);

            var service = new TreasuryExchangeRateApiClient(httpClient);

            // Act
            var result = await service.GetExchangeRatesAsync(
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 31)
            );

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorRegistry.ExternalServiceError.Code, result.Error!.Code);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnFailure_WhenResponseContainsNoData()
        {
            // Arrange
            var json = /*lang=json,strict*/
                """
                {
                    "data": null
                }
                """;

            var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            });

            var httpClient = new HttpClient(handler);

            var service = new TreasuryExchangeRateApiClient(httpClient);

            // Act
            var result = await service.GetExchangeRatesAsync(
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 31)
            );

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorRegistry.ExchangeRateNotFound.Code, result.Error!.Code);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldIncludeTreasuryCurrencyFilter_WhenProvided()
        {
            // Arrange
            HttpRequestMessage? capturedRequest = null;

            var handler = new FakeHttpMessageHandler(request =>
            {
                capturedRequest = request;

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        /*lang=json,strict*/
                        """
                        {
                            "data": []
                        }
                        """,
                        Encoding.UTF8,
                        "application/json"
                    ),
                };
            });

            var httpClient = new HttpClient(handler);

            var service = new TreasuryExchangeRateApiClient(httpClient);

            // Act
            await service.GetExchangeRatesAsync(
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 31),
                "Canada-Dollar"
            );

            // Assert
            Assert.NotNull(capturedRequest);

            string url = capturedRequest!.RequestUri!.ToString();

            Assert.Contains("country_currency_desc:eq:Canada-Dollar", url);
        }
    }
}
