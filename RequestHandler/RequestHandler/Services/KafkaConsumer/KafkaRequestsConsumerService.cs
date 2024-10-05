using Confluent.Kafka;
using RequestHandler.Application;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using RequestHandler.Models;

namespace RequestHandler.Services.Kafka
{
    public class KafkaRequestsConsumerService : IRequestsConsumerService
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly IMessageHandler _messageHandler;

        public KafkaRequestsConsumerService(
            IOptions<KafkaSettings> kafkaSetting,
            IMessageHandler messageHandler)
        {
            _kafkaSettings = kafkaSetting.Value;
            _messageHandler = messageHandler;
        }

        public void StartConsuming(CancellationToken cancellationToken)
        {
            var consumerConfig = new ConsumerConfig()
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = _kafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            consumer.Subscribe(_kafkaSettings.Topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(cancellationToken);
                    if (result != null)
                    {
                        var product = JsonConvert.DeserializeObject<Product>(result.Message.Value);
                        //_logger.LogInformation("--> Success message consume: " + product.Name);
                        _messageHandler.HandleMessage(product);
                        Console.WriteLine("--> Success. Get message: " + product.Name);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                //_logger.LogWarning("!!! Consumer stopped");
            }
            catch (Exception ex)
            {
                //_logger.LogWarning(ex, "An error occurred while consuming messages from Kafka.");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
