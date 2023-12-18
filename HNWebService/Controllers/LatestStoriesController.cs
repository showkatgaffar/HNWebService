using HNWebService.Models;
using HNWebService.Services;
using Microsoft.AspNetCore.Mvc;


namespace HNWebService.Controllers
    {
    /// <summary>
    /// Controller with methods related to latest stories from Hacker News API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LatestStoriesController : ControllerBase
        {
        private readonly IHNApiService _hnSerivce;
        /// <summary>
        /// Injecting HNApi Service to get the latest stories or newest stories
        /// </summary>
        public LatestStoriesController(IHNApiService hackerNewsService)
            {
            _hnSerivce = hackerNewsService;
            }
        /// <summary>
        /// Get method to get the latest stories
        /// </summary>
        [HttpGet("all")]
        public async Task<NewestStoriesModel[]> GetLatestStories()
            {
            var topStories = await _hnSerivce.GetNewestStories();
            return topStories;
            }
        }
    }
