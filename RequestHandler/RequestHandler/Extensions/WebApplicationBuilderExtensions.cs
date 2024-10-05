using StackExchange.Redis;

namespace RequestHandler.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
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
