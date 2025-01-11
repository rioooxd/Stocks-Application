using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksApp.Core.Domain.Entities;
namespace StocksApp.Core.DTO
{
    public class BuyOrderRequest : IOrderRequest
    {

        public Guid BuyOrderID { get; set; }
        [Required(ErrorMessage = "Stock symbol cannot be null")]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock name cannot be null")]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1, 100000)]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
            {
                BuyOrderID = BuyOrderID,
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }
    }
}
