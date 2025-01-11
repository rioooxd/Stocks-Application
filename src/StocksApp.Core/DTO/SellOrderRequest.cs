using StocksApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.DTO
{
    public class SellOrderRequest : IOrderRequest
    {
        public Guid SellOrderID { get; set; }
        [Required]
        public string StockSymbol { get; set; }
        [Required]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 100000)]
        public double Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            {
                SellOrderID = SellOrderID,
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }
    }
}
