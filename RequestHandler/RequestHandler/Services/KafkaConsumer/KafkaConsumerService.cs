using Confluent.Kafka;
using RequestHandler.Application;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RequestHandler.Models;

namespace RequestHandler.Services.Kafka
{
    public class KafkaConsumerService : IConsumerService
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly IMessageHandler _messageHandler;

        public KafkaConsumerService(
            IOptions<KafkaSettings> kafkaSetting,
            IMessageHandler messageHandler)
        {
            _kafkaSettings = kafkaSetting.Value;
            _messageHandler = messageHandler;
        }

        public void StartRequestsConsuming(CancellationToken cancellationToken)
        {
            var consumerConfig = new ConsumerConfig()
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = _kafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            consumer.Subscribe(_kafkaSettings.RequestsTopic);

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
                Console.WriteLine("Requests consumer stopped");
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

        public void StartResponsesConsuming(CancellationToken cancellationToken)
        {
            var consumerConfig = new ConsumerConfig()
            {
                GroupId = _kafkaSettings.GroupId,
                BootstrapServers = _kafkaSettings.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            consumer.Subscribe(_kafkaSettings.ResponsesTopic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(cancellationToken); // List of goods ids
                    if (result != null)
                    {
                        // TODO we need to get all the requests with the same string and send them responses
                        //_messageHandler.HandleResponse();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Requests consumer stopped");
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
