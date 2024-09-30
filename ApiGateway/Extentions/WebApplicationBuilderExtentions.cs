using ApiGateway.MessageBroker.Model;
using Confluent.Kafka;

namespace ApiGateway.Extentions
{
    public static class WebApplicationBuilderExtentions
    {
        public static void AddKafka(this WebApplicationBuilder builder)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = builder.Configuration["Kafka:Host"],
                ClientId = "1"
            };
            builder.Services.AddSingleton(new ProducerBuilder<string, string>
                (producerConfig).Build());
        }
    }
}
