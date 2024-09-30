namespace ApiGateway.MessageBroker.Model
{
    public class RequestData
    {
        /// <summary>
        /// Request identifier
        /// </summary>
        public string QueryId { get; set; }
        /// <summary>
        /// User search string
        /// </summary>
        public string SearchQuery { get; set; }
    }
}
