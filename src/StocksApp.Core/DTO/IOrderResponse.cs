using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.DTO
{
    public interface IOrderResponse
    {
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public OrderType TypeOfOrder { get; }
        public double TradeAmount { get; set; }
    }
    public enum OrderType
    {
        BuyOrder, SellOrder
    }
}
