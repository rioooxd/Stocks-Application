using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using SerilogTimings;
using Microsoft.Extensions.Logging;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.Core.Domain.RepositoryContracts;
namespace StocksApp.Core.Services.FinnhubServices
{
    public class FinnhubStocksSearcherService : IFinnhubStocksSearcherService
    {

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubStocksSearcherService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public FinnhubStocksSearcherService(IFinnhubRepository finnhubRepository, IDiagnosticContext diagnosticContext, ILogger<FinnhubStocksSearcherService> logger)
        {
            _diagnosticContext = diagnosticContext;
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }
        public async Task<Dictionary<string, object>> SearchStocks(string stockSymbol)
        {
            _logger.LogInformation("SearchStocks method of FinnhubService");
            return await _finnhubRepository.SearchStocks(stockSymbol);
        }

    }
}
