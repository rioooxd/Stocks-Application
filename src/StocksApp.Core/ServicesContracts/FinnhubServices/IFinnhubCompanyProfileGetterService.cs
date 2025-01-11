namespace StocksApp.Core.ServicesContracts.FinnhubServices
{
    public interface IFinnhubCompanyProfileGetterService
    {
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
