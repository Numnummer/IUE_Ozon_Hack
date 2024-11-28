using Confluent.Kafka;

namespace ApiGateway.MessageBroker.Publisher
{
    public interface IProductPublisher
    {
        Task ProduceAsync(string topic, Message<string, string> message);
    }

    public class ProductPublisher : IProductPublisher
    {
        private readonly IProducer<string, string> _producer;

        public ProductPublisher(IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public Task ProduceAsync(string topic, Message<string, string> message)
        {
            return _producer.ProduceAsync(topic, message);
        }
    }
}
