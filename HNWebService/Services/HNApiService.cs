using HNWebService.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json;

namespace HNWebService.Services
    {
    /// <summary>
    /// Service for interacting with the Hacker News API.
    /// </summary>
    public class HNApiService: IHNApiService
        {
        private readonly IDistributedCache _cache;
        private readonly HttpClient _httpClient;
        private readonly BaseUrls _baseUrls;
        /// <summary>
        /// Injeting the HttpClient service to make the https requests to Hacker News API's
        /// </summary>
        public HNApiService(HttpClient httpClient, IDistributedCache cache, BaseUrls baseUrls)
            {
            _httpClient = httpClient;
            _cache = cache;
            _baseUrls = baseUrls;
            }   

        /// <summary>
        /// Get newest stories from Hacker News API asynchronously.
        /// </summary>
        public async Task<NewestStoriesModel[]> GetNewestStories()
            {
            try
                {
                // defining cache key 
                const string cacheKey = "NewestItemsCacheKey";

                // get cached item if present
                var cachedItems = await _cache.GetStringAsync(cacheKey);

                // if cached item is not null then data is returned from cache otherwise api is called to fetch data 
                if (!string.IsNullOrEmpty(cachedItems))
                    {
                    return JsonSerializer.Deserialize<NewestStoriesModel[]>(cachedItems);
                    }
                else
                    {
                    // get newstories id from this api
                    var response = await _httpClient.GetStreamAsync($"{_baseUrls.BaseAddress}newstories.json");
                    var newestItemsIds = await JsonSerializer.DeserializeAsync<int[]>(response);

                    var newestItems = new List<NewestStoriesModel>();
                    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                    await Task.WhenAll(newestItemsIds.Select(async itemId =>
                    {
                        var item = await GetHackerNewsItemByIdAsync(itemId);
                        newestItems.Add(item);
                    }));

                    var itemsArray = newestItems.ToArray();

                    // setup the data in cache at first instance 
                    await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(itemsArray), new DistributedCacheEntryOptions
                        {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set cache expiration time
                        });

                    return itemsArray;
                    }
                } catch(Exception ex)
                {
                      throw new Exception(ex.Message);
                }
            
            }
        /// <summary>
        /// Get Get story details by item id.
        /// </summary>
        public async Task<NewestStoriesModel> GetHackerNewsItemByIdAsync(int itemId)
            {
            var response = await _httpClient.GetFromJsonAsync<NewestStoriesModel>($"{_baseUrls.BaseAddress}item/{itemId}.json");
            return response;
            }
        }
    }
