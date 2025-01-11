using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Core.Domain.RepositoryContracts
{
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Gets stock price quote
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns>Dictionary</returns>
        public Task<Dictionary<string, object>?> GetStockPriceQuoute(string stockSymbol);
        /// <summary>
        /// Gets company profile
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns>Dictionary</returns>
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Gets all stocks 
        /// </summary>
        /// <returns>List of dictionary</returns>
        public Task<List<Dictionary<string, string>?>> GetStocks();

        /// <summary>
        /// Searches stock by stockSymbol
        /// </summary>
        /// <param name="stockSymbolToSearch"></param>
        /// <returns>Dictionary</returns>
        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);

    }
}
