using Moq;
using HNWebService.Services;
using HNWebService.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;
using Moq.Protected;
using System.Text;

namespace HNWebServiceUnitTest.Unit
    {
    [TestFixture]
    public class HackerNewsServiceTests
        {
        /// <summary>
        /// Handled test case of HackerNewsItemById method only as we have itemId mock data available
        /// </summary>
        [Test]
        public async Task GetHackerNewsItemByIdAsync_ShouldReturnItemWithUrl()
            {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHandler.Object)
                {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/") // Set a base address
                };

            var mockCache = new Mock<IDistributedCache>();
            var hackerNewsService = new HNApiService(httpClient, mockCache.Object);

            var expectedItem = new NewestStoriesModel { id = 1, title = "Item with URL", url = "http://example.com" };
            var itemId = 1;

            var json = JsonSerializer.Serialize(expectedItem);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                Content = new StringContent(json)
                };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await hackerNewsService.GetHackerNewsItemByIdAsync(itemId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedItem.id, result.id);
            Assert.AreEqual(expectedItem.title, result.title);
            Assert.AreEqual(expectedItem.url, result.url);
            }
        }
    }
