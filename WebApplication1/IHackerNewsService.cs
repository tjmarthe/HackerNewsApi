namespace WebApplication1
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<NewsStory>> GetStoryList();
    }
}