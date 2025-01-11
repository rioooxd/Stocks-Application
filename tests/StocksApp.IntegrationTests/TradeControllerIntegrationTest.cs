using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.IntegrationTests
{

    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Index_ShouldReturnView()
        {
            //Act
            HttpResponseMessage response = await _client.GetAsync("/Trade/Index");

            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();

            html.LoadHtml(responseBody);

            var document = html.DocumentNode;

            document.QuerySelectorAll("div.trading-panel").Should().NotBeNull();
            document.QuerySelectorAll("div.new-order-panel").Should().NotBeNull();
            document.QuerySelectorAll(".price").Should().NotBeNull();
        }
    }
}
