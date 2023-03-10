using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Core
{
    class TradingEngineServer : BackgroundService, ITradingEngineServer
    {

        private readonly IOptions<TradingEngineServerConfiguration> _engineConfiguration;
        private readonly ITextLogger _logger;

        public TradingEngineServer(IOptions<TradingEngineServerConfiguration> engineConfiguration,
            ITextLogger textLogger)
        {
            _engineConfiguration = engineConfiguration ?? throw new ArgumentNullException(nameof(engineConfiguration));
            _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);
      
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information(nameof(TradingEngineServer), "Starting Trading Engine");
            while (!stoppingToken.IsCancellationRequested)
            {

            }
            _logger.Information(nameof(TradingEngineServer), "Stopping Trading Engine");
            return Task.CompletedTask;
        }
    }
}
