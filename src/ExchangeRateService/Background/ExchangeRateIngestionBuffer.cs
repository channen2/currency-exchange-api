using System.Threading.Channels;
using ExchangeRateService.Background.Interfaces;

namespace ExchangeRateService.Background
{
    public class ExchangeRateIngestionBuffer : IExchangeRateIngestionBuffer
    {
        private readonly Channel<(DateTime from, DateTime to)> _channel;

        public ExchangeRateIngestionBuffer()
        {
            _channel = Channel.CreateBounded<(DateTime, DateTime)>(
                new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.Wait }
            );
        }

        public async ValueTask EnqueueAsync(DateTime fromDate, DateTime toDate)
        {
            await _channel.Writer.WriteAsync((fromDate, toDate));
        }

        public async ValueTask<(DateTime from, DateTime to)> DequeueAsync(CancellationToken ct)
        {
            return await _channel.Reader.ReadAsync(ct);
        }
    }
}
