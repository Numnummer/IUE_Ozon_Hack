using ApiGateway.MessageBroker.Consumer;
using ApiGateway.MessageBroker.Model;
using MassTransit;

namespace ApiGateway.Extentions
{
    public static class WebApplicationBuilderExtentions
    {
        public static void AddMessageBroker(this WebApplicationBuilder builder)
        {
            var kafkaHost = builder.Configuration["Kafka:Host"];
            builder.Services.AddMassTransit(x =>
            {
                x.UsingInMemory();
                x.AddRider(rider =>
                {
                    rider.AddConsumer<ProductConsumer>();
                    rider.AddProducer<SearchModel>("search-model");
                    rider.UsingKafka((context, config) =>
                    {
                        config.Host(kafkaHost);
                        config.TopicEndpoint<Product>("product", "asd1", e =>
                        {
                            e.ConfigureConsumer<ProductConsumer>(context);
                        });
                    });
                });
            });
        }
    }
}
