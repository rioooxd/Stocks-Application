using StocksApp.Core.Domain.Entities;

namespace StocksApp.Core.Domain.RepositoryContracts
{
    public interface IStocksRepository
    {
        /// <summary>
        /// Creates a new buy order
        /// </summary>
        /// <param name="buyOrder"></param>
        /// <returns>BuyOrder</returns>
        public Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        /// <summary>
        /// Creates a new sell order
        /// </summary>
        /// <param name="sellOrder"></param>
        /// <returns>SellOrder</returns>
        public Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Gets list of all buy orders
        /// </summary>
        /// <returns>List of buy orders</returns>
        public Task<List<BuyOrder>> GetBuyOrders();

        /// <summary>
        /// Gets list of all sells orders
        /// </summary>
        /// <returns>List of sell orders</returns>
        public Task<List<SellOrder>> GetSellOrders();
    }
}
