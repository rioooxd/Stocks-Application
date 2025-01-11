using StocksApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.ServicesContracts.StocksServices
{
    public interface IBuyOrdersService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
