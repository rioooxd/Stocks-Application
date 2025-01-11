using StocksApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StocksApp.Core.Services.StocksServices;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp.Core.Services.FinnhubServices;
using StocksApp.Core.ServicesContracts.StocksServices;
using StocksApp.UI.Filters.ActionFilters;
using StocksApp.Infrastructure.Repositories;
using StocksApp.Infrastructure.AppDbContext;
namespace StocksApp.UI.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddTransient<CreateOrderActionFilter>();

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
            services.AddScoped<IFinnhubCompanyProfileGetterService, FinnhubCompanyProfileGetterService>();
            services.AddScoped<IFinnhubStockPriceQuoteGetterService, FinnhubStockPriceQuoteGetterService>();
            services.AddScoped<IFinnhubStocksGetterService, FinnhubStocksGetterService>();
            services.AddScoped<IFinnhubStocksSearcherService, FinnhubStocksSearcherService>();
            services.AddScoped<IBuyOrdersService, BuyOrdersService>();
            services.AddScoped<ISellOrdersService, SellOrdersService>();


            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
                options.EnableSensitiveDataLogging();
            });
            return services;
        }
    }
}
