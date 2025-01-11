using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Core.Services;
using System.Diagnostics;
using Rotativa.AspNetCore;
using StocksApp.UI.Filters;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.Core.DTO;
using StocksApp.Core.ServicesContracts.StocksServices;
using StocksApp.UI.Models;
using StocksApp.UI;
using StocksApp.UI.Filters.ActionFilters;

namespace StocksApp.UI.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubStockPriceQuoteGetterService _finnhubStockPriceQuoteGetterService;
        private readonly IFinnhubCompanyProfileGetterService _finnhubCompanyProfileGetterService;
        private readonly IFinnhubStocksGetterService _finnhubStocksGetterService;
        private readonly IFinnhubStocksSearcherService _finnhubStocksSearcherService;
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubStockPriceQuoteGetterService finnhubStockPriceQuoteGetterService, IFinnhubCompanyProfileGetterService finnhubCompanyProfileGetterService, IFinnhubStocksGetterService finnhubStocksGetterService, IFinnhubStocksSearcherService finnhubStocksSearcherService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IBuyOrdersService buyOrdersService, ISellOrdersService sellOrdersService, ILogger<TradeController> logger)
        {
            _finnhubCompanyProfileGetterService = finnhubCompanyProfileGetterService;
            _finnhubStockPriceQuoteGetterService = finnhubStockPriceQuoteGetterService;
            _finnhubStocksGetterService = finnhubStocksGetterService;
            _finnhubStocksSearcherService = finnhubStocksSearcherService;
            _tradingOptions = tradingOptions;
            _configuration = configuration;
            _buyOrdersService = buyOrdersService;
            _sellOrdersService = sellOrdersService;
            _logger = logger;
        }


        [Route("[action]/{stockSymbol?}")]
        [Route("/")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            _logger.LogInformation("Index action method of TradeController");

            if (stockSymbol == null)
            {
                stockSymbol = "MSFT";
            }
            _logger.LogDebug("Stock symbol: " + stockSymbol);

            //msft
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubStockPriceQuoteGetterService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileGetterService.GetCompanyProfile(stockSymbol);

            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = stockSymbol,
            };

            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]), StockName = Convert.ToString(companyProfileDictionary["name"]), Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            return View(stockTrade);
        }
        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = await _buyOrdersService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _sellOrdersService.GetSellOrders();

            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradingOptions;
            return View(orders);
        }
        [Route("[action]")]
        [HttpPost]
        [CreateOrderFilterFactory()]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            orderRequest.DateAndTimeOfOrder = DateTime.Now;


            await _sellOrdersService.CreateSellOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }
        [Route("[action]")]
        [HttpPost]
        [CreateOrderFilterFactory()]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            await _buyOrdersService.CreateBuyOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> StocksPDF()
        {

            List<BuyOrderResponse> buyOrderResponses = await _buyOrdersService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _sellOrdersService.GetSellOrders();

            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradingOptions;
            return new ViewAsPdf("StocksPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20,
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}