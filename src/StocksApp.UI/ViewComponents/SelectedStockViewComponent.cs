using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.Core.ServicesContracts.StocksServices;
using StocksApp.UI;

namespace StocksApp.UI.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IBuyOrdersService _stocksService;
        private readonly IFinnhubStockPriceQuoteGetterService _finnhubStockPriceQuoteGetterService;
        private readonly IFinnhubCompanyProfileGetterService _finnhubCompanyProfileGetterService;
        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IBuyOrdersService stocksService, IFinnhubCompanyProfileGetterService finnhubCompanyProfileGetterService, IFinnhubStockPriceQuoteGetterService finnhubStockPriceQuoteGetterService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksService = stocksService;
            _finnhubCompanyProfileGetterService = finnhubCompanyProfileGetterService;
            _finnhubStockPriceQuoteGetterService = finnhubStockPriceQuoteGetterService;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stockSymbol != null)
            {
                companyProfileDict = await _finnhubCompanyProfileGetterService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubStockPriceQuoteGetterService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }
    }
}
