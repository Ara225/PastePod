using System;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace PastePodUnitTests
{
    [Collection("Sequential")]
    public class FunctionalTests : IClassFixture<WebTestFixture>
    {
        public FunctionalTests(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Fact]
        public async Task CanGetIndexPage()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("Welcome", stringResponse);
        }
    }
}
