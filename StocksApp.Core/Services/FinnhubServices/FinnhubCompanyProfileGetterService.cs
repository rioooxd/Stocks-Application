using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using SerilogTimings;
using Microsoft.Extensions.Logging;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.Core.Domain.RepositoryContracts;
namespace StocksApp.Core.Services.FinnhubServices
{
    public class FinnhubCompanyProfileGetterService : IFinnhubCompanyProfileGetterService
    {

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubCompanyProfileGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public FinnhubCompanyProfileGetterService(IFinnhubRepository finnhubRepository, IDiagnosticContext diagnosticContext, ILogger<FinnhubCompanyProfileGetterService> logger)
        {
            _diagnosticContext = diagnosticContext;
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("GetCompanyProfile method of FinnhubService");
            Dictionary<string, object>? companyProfile = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            return companyProfile;
        }

    }
}
