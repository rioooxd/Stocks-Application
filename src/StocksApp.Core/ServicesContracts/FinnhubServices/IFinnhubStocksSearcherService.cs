namespace StocksApp.Core.ServicesContracts.FinnhubServices
{
    public interface IFinnhubStocksGetterService
    {
        public Task<List<Dictionary<string, string>>> GetStocks();
    }
}
