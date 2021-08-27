namespace Bud.Api.IntegrationTests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class CountryEndpointTests : IClassFixture<WebApplicationFactory<Bud.Api.Startup>>
    {
        private readonly WebApplicationFactory<Bud.Api.Startup> _factory;

        public CountryEndpointTests(WebApplicationFactory<Bud.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/countries/gb")]
        [InlineData("/countries/GBR")]
        public async Task Countries_ReturnsSuccessStatusCode(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/countries/123")]
        public async Task Countries_ReturnsUnprocessableEntityStatusCode(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }
    }
}