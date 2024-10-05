namespace RequestHandler.Services
{
    public class KafkaSettings
    {
        public string GroupId { get; set; }
        public string BootstrapServers { get; set; }
        public string RequestsTopic { get; set; }
        public string HandledRequestsTopic { get; set; }
        public string ResponsesTopic { get; set; }
    }
}
