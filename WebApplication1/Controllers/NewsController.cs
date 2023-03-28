using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        public readonly IHackerNewsService hackerNewsService;
      
        public NewsController(IHackerNewsService hackerNewsService)
        {
            this.hackerNewsService= hackerNewsService;
        }

        [HttpGet(Name = "GetHackerNewsList")]
        public async Task<ActionResult<IEnumerable<NewsStory>>> Get() =>
           Ok(await hackerNewsService.GetStoryList());
    }
}