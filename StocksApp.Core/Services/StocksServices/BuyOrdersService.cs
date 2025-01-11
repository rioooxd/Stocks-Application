using StocksApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Hosting;
using SerilogTimings;
using Serilog;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Core.ServicesContracts.StocksServices;
using StocksApp.Core.Helpers;
using StocksApp.Core.DTO;
namespace StocksApp.Core.Services.StocksServices
{
    public class BuyOrdersService : IBuyOrdersService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<BuyOrdersService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public BuyOrdersService(IStocksRepository stocksRepository, ILogger<BuyOrdersService> logger, IDiagnosticContext diagnosticContext)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation("CreateBuyOrder of StocksService");
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));
            if (buyOrderRequest.StockSymbol == null) throw new ArgumentException(nameof(buyOrderRequest));
            if (buyOrderRequest.DateAndTimeOfOrder < DateTime.Parse("01/01/1900")) throw new ArgumentException(nameof(buyOrderRequest));
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            buyOrder.BuyOrderID = Guid.NewGuid();

            await _stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();

        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            _logger.LogInformation("GetBuyOrders of StocksService");
            List<BuyOrder> buyOrders;
            using (Operation.Time("Time elapsed for GetBuyOrders from Database"))
            {
                buyOrders = await _stocksRepository.GetBuyOrders();
            }
            _diagnosticContext.Set("BuyOrders", buyOrders);
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }
    }
}
