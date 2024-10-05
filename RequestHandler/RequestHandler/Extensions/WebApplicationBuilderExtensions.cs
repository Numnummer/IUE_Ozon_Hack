using StackExchange.Redis;
using RequestHandler.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace RequestHandler.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddKafkaProducer(this WebApplicationBuilder builder)
        {
            var kafkaSettings = builder.Services.BuildServiceProvider()
                .GetRequiredService<IOptions<KafkaSettings>>().Value;

            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                ClientId = "1"
            };

            builder.Services.AddSingleton(new ProducerBuilder<Null, string>
                (producerConfig).Build());
        }

        public static void AddRedisConnetionMultiplexer(this WebApplicationBuilder builder)
        {
            var redisHost = builder.Configuration["Redis:Host"];
            if (String.IsNullOrEmpty(redisHost))
            {
                throw new ArgumentException("Redis host is not set");
            }
            var redis = ConnectionMultiplexer.Connect(redisHost);

            builder.Services.AddSingleton(redis);
        }
    }
}
