using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RequestHandler.Application;

namespace RequestHandler.Services.KafkaProducers
{
    public class KafkaProducerService : IProduceService
    {
        private readonly IProducer<Null, string> _handledRequestsProducer;
        private readonly KafkaSettings _kafkaSettings;
        public KafkaProducerService(IProducer<Null, string> handledRequestsProducer,
            IOptions<KafkaSettings> kafkaSettings)
        {
            _handledRequestsProducer = handledRequestsProducer;
            _kafkaSettings = kafkaSettings.Value;
        }

        public Task ProduceHandledRequest(string request)
        {
            var message = new Message<Null, string>()
            {
                Value = JsonConvert.SerializeObject(request)
            };
            Console.WriteLine($"Producer send handled request: {request}");
            return _handledRequestsProducer.ProduceAsync(
                _kafkaSettings.HandledRequestsTopic, message);
        }
    }
}
