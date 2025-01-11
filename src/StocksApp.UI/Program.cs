using StocksApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StocksApp.Core.Domain.RepositoryContracts;
using StocksApp;
using Serilog;
using Serilog.AspNetCore;
using StocksApp.UI.Filters.ActionFilters;
using StocksApp.UI.Middleware;
using StocksApp.Core.Services.StocksServices;
using StocksApp.Core.Services.FinnhubServices;
using StocksApp.Core.ServicesContracts.FinnhubServices;
using StocksApp.Core.ServicesContracts.StocksServices;
using StocksApp.UI.StartupExtensions;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});
//Services

builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();
app.UseSerilogRequestLogging();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
if (!builder.Environment.IsEnvironment("Test"))
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program { }