namespace StocksApp.Core.ServicesContracts.FinnhubServices
{
    public interface IFinnhubStocksSearcherService
    {
        public Task<Dictionary<string, object>> SearchStocks(string stockSymbol);
    }
}
