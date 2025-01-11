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
using StocksApp.Core.DTO;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Core.Helpers;
using StocksApp.Core.ServicesContracts.StocksServices;
namespace StocksApp.Core.Services.StocksServices
{
    public class SellOrdersService : ISellOrdersService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<SellOrdersService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public SellOrdersService(IStocksRepository stocksRepository, ILogger<SellOrdersService> logger, IDiagnosticContext diagnosticContext)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            _logger.LogInformation("CreateSellOrder of StocksService");
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));
            if (sellOrderRequest.StockSymbol == null) throw new ArgumentException(nameof(sellOrderRequest));
            if (sellOrderRequest.DateAndTimeOfOrder < DateTime.Parse("01/01/1900")) throw new ArgumentException(nameof(sellOrderRequest));
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();


            await _stocksRepository.CreateSellOrder(sellOrder);
            return sellOrder.ToSellOrderResponse();
        }
        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            _logger.LogInformation("GetSellOrders of StocksService");
            List<SellOrder> sellOrders;
            using (Operation.Time("Time elapsed for GetSellOrders from Database"))
            {
                sellOrders = await _stocksRepository.GetSellOrders();
            }
            _diagnosticContext.Set("BuyOrders", sellOrders);
            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
