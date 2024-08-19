using ApiGateway.MessageBroker.Model;
using MassTransit;

namespace ApiGateway.MessageBroker.Consumer
{
    public class ProductConsumer : IConsumer<Product>
    {
        public Task Consume(ConsumeContext<Product> context)
        {
            throw new NotImplementedException();
        }
    }
}
