namespace HNWebService.Models
    {
    /// <summary>
    /// NewestStoriesModel class is used to create the response from the newest stories api
    /// </summary>
    public class NewestStoriesModel
        {
        /// <summary>
        /// Gets the id field from item api of the Hacker News this Id can be used for further processing
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Gets the title field from item api of the Hacker News 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Gets the url field from item api of the Hacker News 
        /// </summary>
        public string url { get; set; }
        }
    }
