using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using SerilogTimings;
using Microsoft.Extensions.Logging;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Core.ServicesContracts.FinnhubServices;
namespace StocksApp.Core.Services.FinnhubServices
{
    public class FinnhubStockPriceQuoteGetterService : IFinnhubStockPriceQuoteGetterService
    {

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubStockPriceQuoteGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public FinnhubStockPriceQuoteGetterService(IFinnhubRepository finnhubRepository, IDiagnosticContext diagnosticContext, ILogger<FinnhubStockPriceQuoteGetterService> logger)
        {
            _diagnosticContext = diagnosticContext;
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }
        public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
        {
            _logger.LogInformation("GetStockPriceQuote method of FinnhubService");
            Dictionary<string, object>? stockPriceQuoute = await _finnhubRepository.GetStockPriceQuoute(stockSymbol);

            return stockPriceQuoute;
        }

    }
}
