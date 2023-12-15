public class HNApiService
    {
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";

    public HNApiService(HttpClient httpClient)
        {
        _httpClient = httpClient;
        }
    // API to get latest stories from Hacker News API
    public async Task<List<int>> GetTopStoriesAsync()
        {
        var response = await _httpClient.GetFromJsonAsync<List<int>>(BaseUrl + "topstories.json");
        return response ?? new List<int>();
        }
    }

