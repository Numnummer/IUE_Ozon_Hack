namespace RequestHandler.Services.Kafka
{
    public class KafkaSettings
    {
        public string GroupId { get; set; }
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
    }
}
