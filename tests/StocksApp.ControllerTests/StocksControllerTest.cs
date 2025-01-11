using Microsoft.Extensions.Options;
using StocksApp.Core.ServicesContracts;
using StocksApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using StocksApp.UI.Controllers;
using FluentAssertions;
using StocksApp.UI.Models;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using Castle.Core.Logging;
using StocksApp.Core.Services.FinnhubServices;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using StocksApp.UI;
namespace StocksApp.ControllerTests
{
    public class StocksControllerTest
    {
        private readonly IFinnhubStocksGetterService _finnhubStocksGetterService;

        private readonly Mock<IFinnhubStocksGetterService> _finnhubStocksGetterServiceMock;

        private readonly Mock<ILogger<StocksController>> _loggerMock;
        private Fixture _fixture;

        public StocksControllerTest()
        {
            _fixture = new Fixture();

            _finnhubStocksGetterServiceMock = new Mock<IFinnhubStocksGetterService>();
            _finnhubStocksGetterService = _finnhubStocksGetterServiceMock.Object;

            _loggerMock = new Mock<ILogger<StocksController>>();
        }

        [Fact]
        public async Task Explore_StocksIsNull_ShouldReturnView()
        {
            IOptions<TradingOptions> options = Options.Create(new TradingOptions()
            {
                DefaultOrderQuantity = 100,
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO"
            });
            List<Dictionary<string, string>>? stocksDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");


            StocksController stocksController = new StocksController(_finnhubStocksGetterService, options, _loggerMock.Object);


            List<Stock> stocksList = stocksDictionary.Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
               .ToList();

            _finnhubStocksGetterServiceMock.Setup(temp => temp.GetStocks())
                .ReturnsAsync(stocksDictionary);

            IActionResult actionResult = await stocksController.Explore(null);

            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(stocksList);
        }
    }
}
