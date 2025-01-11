using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Core.ServicesContracts;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.UI;
using StocksApp.UI.Models;

namespace StocksApp.UI.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubStocksGetterService _finnhubStocksGetterService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly ILogger<StocksController> _logger;
        public StocksController(IFinnhubStocksGetterService finnhubStocksGetterService, IOptions<TradingOptions> tradingOptions, ILogger<StocksController> logger)
        {
            _finnhubStocksGetterService = finnhubStocksGetterService;
            _tradingOptions = tradingOptions;
            _logger = logger;
        }
        [Route("[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            _logger.LogInformation("Explore method of StocksController");
            _logger.LogDebug("Stock: " + stock + "showAll: " + showAll);
            List<Dictionary<string, string>> stocksDictionary = await _finnhubStocksGetterService.GetStocks();
            List<Stock> stocks = new List<Stock>();
            if (stocksDictionary != null)
            {
                if (!showAll && _tradingOptions.Value.Top25PopularStocks != null)
                {
                    string[]? Top25 = _tradingOptions.Value.Top25PopularStocks.Split(',');
                    if (Top25 != null)
                    {
                        stocksDictionary = stocksDictionary.Where(temp => Top25.Contains(Convert.ToString(temp["symbol"])))
                            .ToList();
                    }
                }
            }

            stocks = stocksDictionary
                .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
               .ToList();

            ViewBag.stock = stock;
            return View(stocks);
        }

    }
}
