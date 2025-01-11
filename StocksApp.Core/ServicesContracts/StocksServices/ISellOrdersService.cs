using StocksApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.ServicesContracts.StocksServices
{
    public interface ISellOrdersService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
