using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace WebApplication1
{
    public class HackerNewsService : IHackerNewsService
    {
        public HttpClient _httpClient;
        public IMemoryCache Cache;

        const string hackerNewsUrl = "https://hacker-news.firebaseio.com/v0/";
        const string cacheKey = "news_List";
        public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
        {
            this._httpClient = httpClient;
            this.Cache = cache;
        }

        public async Task<IEnumerable<NewsStory>> GetStoryList()
        { 
            var result = new List<NewsStory>();
            Cache.TryGetValue(cacheKey, out result);
            if (result == null)
            {

                var response = await _httpClient.GetAsync(hackerNewsUrl + "topstories.json");
                var idString = response.Content.ReadAsStringAsync().Result;
                var newsListIds = JsonConvert.DeserializeObject<List<string>>(idString);

                result = new List<NewsStory>();
                
                foreach (var id in newsListIds)
                {
                    string requestUri = $"{hackerNewsUrl}/item/{id}.json";
                    var storyFull = await _httpClient.GetAsync(requestUri);
                    var story = JsonConvert.DeserializeObject<NewsStory>(storyFull.Content.ReadAsStringAsync().Result);

                    result.Add(story);
                }


                Cache.Set(cacheKey, result, TimeSpan.FromSeconds(300));
            }

            return result.AsEnumerable();

        }
    }
}
