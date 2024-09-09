
using ApiGateway.MessageBroker.Model;
using Confluent.Kafka;
using Newtonsoft.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace ApiGateway.MessageBroker.Consumer
{
    public class ProductConsumer(IConsumer<string, string> consumer)
        : IHostedService
    {
        private readonly IConsumer<string, string> _consumer = consumer;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("product");

            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
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
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Unsubscribe();
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
