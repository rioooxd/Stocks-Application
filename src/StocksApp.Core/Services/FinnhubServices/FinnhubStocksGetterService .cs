using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using SerilogTimings;
using Microsoft.Extensions.Logging;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Core.ServicesContracts.FinnhubServices;
namespace StocksApp.Core.Services.FinnhubServices
{
    public class FinnhubStocksGetterService : IFinnhubStocksGetterService
    {

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubStocksGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public FinnhubStocksGetterService(IFinnhubRepository finnhubRepository, IDiagnosticContext diagnosticContext, ILogger<FinnhubStocksGetterService> logger)
        {
            _diagnosticContext = diagnosticContext;
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }
        public async Task<List<Dictionary<string, string>>> GetStocks()
        {
            _logger.LogInformation("GetStocks method of FinnhubService");
            List<Dictionary<string, string>> stocks;
            using (Operation.Time("Time elapsed for GetStocks from Database"))
            {
                stocks = await _finnhubRepository.GetStocks();
            }
            _diagnosticContext.Set("Stocks", stocks);
            return stocks;
        }

    }
}
