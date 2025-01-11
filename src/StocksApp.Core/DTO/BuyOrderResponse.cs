using StocksApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }
        public string StockSymbol { get; set; }
        [Required]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType TypeOfOrder { get; } = OrderType.BuyOrder;
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(BuyOrderResponse))
            {
                return false;
            }
            BuyOrderResponse order_to_compare = (BuyOrderResponse)obj;
            return BuyOrderID == order_to_compare.BuyOrderID && StockSymbol == order_to_compare.StockSymbol && StockName == order_to_compare.StockName && DateAndTimeOfOrder == order_to_compare.DateAndTimeOfOrder && Quantity == order_to_compare.Quantity && Price == order_to_compare.Price;
        }
    }
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                TradeAmount = buyOrder.Quantity * buyOrder.Price,
            };
        }
    }
}
