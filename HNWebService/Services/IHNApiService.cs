using HNWebService.Models;

namespace HNWebService.Services
    {
    /// <summary>
    /// Interface for Dependency Injection
    /// </summary>
    public interface IHNApiService
        {
        /// <summary>
        /// GetNewestStories method to get bewest stories
        /// </summary>
        Task<NewestStoriesModel[]> GetNewestStories();
        }
    }
