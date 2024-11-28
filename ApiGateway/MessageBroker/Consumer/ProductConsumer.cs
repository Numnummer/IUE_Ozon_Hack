
using ApiGateway.MessageBroker.Model;
using Confluent.Kafka;
using Newtonsoft.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ApiGateway.MessageBroker.Consumer
{
    public class ProductConsumer(IConsumer<string, string> consumer)
        : BackgroundService
    {
        //private readonly IConsumer<string, string> _consumer = consumer;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //consumer.Subscribe("product");

            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult is null)
                    {
                        return;
                    }
                    var product = JsonConvert.DeserializeObject<Product>(consumeResult.Message.Value);
                    if (product is null)
                    {
                        return;
                    }
                    var url = product.RedirectUrl;
                    var httpClient = new HttpClient();
                    await httpClient.GetAsync(url);
                }
            }, stoppingToken);
        }
    }
}
