using ApiGateway.MessageBroker.Model;
using MassTransit;

namespace ApiGateway.MessageBroker.Consumer
{
    public class ProductConsumer : IConsumer<Product>
    {
        public async Task Consume(ConsumeContext<Product> context)
        {
            var url = context.Message.RedirectUrl;
            var httpClient = new HttpClient();
            await httpClient.GetAsync(url);
        }
    }
}
