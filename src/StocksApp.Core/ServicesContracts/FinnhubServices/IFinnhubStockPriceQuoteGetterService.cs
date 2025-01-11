namespace StocksApp.Core.ServicesContracts.FinnhubServices
{
    public interface IFinnhubStockPriceQuoteGetterService
    {
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
