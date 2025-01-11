using AutoFixture;
using Castle.Core.Logging;
using StocksApp.Core.Domain.Entities;
using FluentAssertions;
using Moq;
using StocksApp.Core.Domain.RepositoryContracts;
using Serilog;
using StocksApp.Core.Services.StocksServices;
using StocksApp.Core.DTO;
using StocksApp.Core.ServicesContracts.StocksServices;
using Xunit.Abstractions;
using Serilog;
using Serilog.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace StocksApp.ServicesTests
{
    public class StocksServiceTest
    {
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;
        private readonly ITestOutputHelper _outputHelper;
        private readonly IStocksRepository _stocksRepository;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly Fixture _fixture;
        public StocksServiceTest(ITestOutputHelper outputHelper)
        {
            _fixture = new Fixture();

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            var diagnosticContextMock = new Mock<IDiagnosticContext>();
            var buyOrdersServiceLoggerMock = new Mock<ILogger<BuyOrdersService>>();
            var sellOrdersServiceLoggerMock = new Mock<ILogger<SellOrdersService>>();
            _buyOrdersService = new BuyOrdersService(_stocksRepository, buyOrdersServiceLoggerMock.Object, diagnosticContextMock.Object);
            _sellOrdersService = new SellOrdersService(_stocksRepository, sellOrdersServiceLoggerMock.Object, diagnosticContextMock.Object);

            _outputHelper = outputHelper;
        }
        #region CreateBuyOrder
        [Fact]
        public async Task CreateBuyOrder_ToBeArgumentNullException()
        {
            BuyOrderRequest? request = null;
            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();


        }
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_QuantityLessThanMinimum_ToBeArgumentException(uint quantity)
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, quantity)
                .Create();

            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateBuyOrder_QuantityMoreThanMaximum_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001)
                .Create();

            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateBuyOrder_PriceLessThanMinimum_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, 0)
                .Create();

            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateBuyOrder_PriceMoreThanMaximum()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, 100001)
                .Create();

            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }
        [Fact]
        public async Task CreateBuyOrder_DateTooOld_ToBeArgumentException()
        {
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("01/01/1899"))
                .Create();

            Func<Task> action = async () =>
            {
                await _buyOrdersService.CreateBuyOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }
        [Fact]
        public async Task CreateBuyOrder_ValidRequest_ToBeSuccessful()
        {
            BuyOrder buyOrder = _fixture.Create<BuyOrder>();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            BuyOrderRequest buyOrderRequest = _fixture.Create<BuyOrderRequest>();

            BuyOrderResponse buyOrderResponse = await _buyOrdersService.CreateBuyOrder(buyOrderRequest);

            buyOrderResponse.BuyOrderID.Should().NotBe(Guid.Empty);

        }
        #endregion
        #region GetBuyOrders
        [Fact]
        public async Task GetBuyOrders_EmptyList_ToBeEmpty()
        {
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());

            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders();

            buyOrders.Should().BeEmpty();
        }
        [Fact]
        public async Task GetBuyOrders_ValidList_ToBeSuccessful()
        {

            List<BuyOrder> buyOrders = _fixture.Create<List<BuyOrder>>();

            List<BuyOrderResponse> buyOrderResponses = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrders_fromGetBuyOrders = await _buyOrdersService.GetBuyOrders();

            buyOrders_fromGetBuyOrders.Should().BeEquivalentTo(buyOrderResponses);
        }
        #endregion

        #region CreateSellOrder
        [Fact]
        public async Task CreateSellOrder_ToBeArgumentNullException()
        {
            SellOrderRequest? request = null;
            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();


        }
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_QuantityLessThanMinimum_ToBeArgumentException(uint quantity)
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, quantity)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateSellOrder_QuantityMoreThanMaximum_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, (uint)100001)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateSellOrder_PriceLessThanMinimum_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, 0)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        public async Task CreateSellOrder_PriceMoreThanMaximum()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, 100001)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }
        [Fact]
        public async Task CreateSellOrder_DateTooOld_ToBeArgumentException()
        {
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("01/01/1899"))
                .Create();

            Func<Task> action = async () =>
            {
                await _sellOrdersService.CreateSellOrder(request);
            };
            await action.Should().ThrowAsync<ArgumentException>();

        }
        [Fact]
        public async Task CreateSellOrder_ValidRequest_ToBeSuccessful()
        {
            SellOrder SellOrder = _fixture.Create<SellOrder>();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(SellOrder);

            SellOrderRequest SellOrderRequest = _fixture.Create<SellOrderRequest>();

            SellOrderResponse SellOrderResponse = await _sellOrdersService.CreateSellOrder(SellOrderRequest);

            SellOrderResponse.SellOrderID.Should().NotBe(Guid.Empty);

        }
        #endregion
        #region GetSellOrders
        [Fact]
        public async Task GetSellOrders_EmptyList_ToBeEmpty()
        {
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());

            List<SellOrderResponse> SellOrders = await _sellOrdersService.GetSellOrders();

            SellOrders.Should().BeEmpty();
        }
        [Fact]
        public async Task GetSellOrders_ValidList_ToBeSuccessful()
        {

            List<SellOrder> SellOrders = _fixture.Create<List<SellOrder>>();

            List<SellOrderResponse> SellOrderResponses = SellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(SellOrders);

            List<SellOrderResponse> SellOrders_fromGetSellOrders = await _sellOrdersService.GetSellOrders();

            SellOrders_fromGetSellOrders.Should().BeEquivalentTo(SellOrderResponses);
        }
        #endregion
    }
}