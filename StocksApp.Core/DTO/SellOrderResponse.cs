using StocksApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.DTO
{
    public class SellOrderResponse : IOrderResponse
    {
        public Guid SellOrderID { get; set; }
        [Required]
        public string StockSymbol { get; set; }
        [Required]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public OrderType TypeOfOrder { get; } = OrderType.SellOrder;
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(SellOrderResponse))
            {
                return false;
            }
            SellOrderResponse order_to_compare = (SellOrderResponse)obj;
            return SellOrderID == order_to_compare.SellOrderID && StockSymbol == order_to_compare.StockSymbol && StockName == order_to_compare.StockName && DateAndTimeOfOrder == order_to_compare.DateAndTimeOfOrder && Quantity == order_to_compare.Quantity && Price == order_to_compare.Price;
        }
    }
    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
