using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp.Core.DTO;
using StocksApp.UI.Controllers;
using StocksApp.UI.Models;

namespace StocksApp.UI.Filters.ActionFilters
{
    public class CreateOrderFilterFactory : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<CreateOrderActionFilter>();
            return filter;
        }
    }
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<CreateOrderActionFilter> _logger;
        public CreateOrderActionFilter(ILogger<CreateOrderActionFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //before
            _logger.LogInformation("Before CreateOrderActionFilter");
            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;
                if (orderRequest != null)
                {
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);
                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, StockSymbol = orderRequest.StockSymbol, Price = orderRequest.Price, Quantity = orderRequest.Quantity };

                        context.Result = tradeController.View("Index", stockTrade);
                    }
                    else
                    {
                        await next();
                    }

                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();

            }
            //after
            _logger.LogInformation("After CreateOrderActionFilter");
        }
    }
}
