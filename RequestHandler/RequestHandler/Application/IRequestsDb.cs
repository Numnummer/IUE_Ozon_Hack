namespace RequestHandler.Application
{
    public interface IRequestsDb
    {
        /// <summary>
        /// Returns true if such request already processing
        /// </summary>
        /// <param name="key" - request string></param>
        /// <returns></returns>
        public bool IsRequestProcessing(string key);
        /// <summary>
        /// Get new request from user and process it, check if such request already in processing
        /// Add queryId to awaiting list
        /// Send to processing if it not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<long> AddNewRequest(string key, string queryId);
        /// <summary>
        /// Remove this request from DB and send response to the request
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string FinishRequest(string key);
    }
}
