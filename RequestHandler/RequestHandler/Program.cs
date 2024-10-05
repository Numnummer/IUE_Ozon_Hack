using RequestHandler.Application;
using RequestHandler.Extensions;
using RequestHandler.Repositories;
using RequestHandler.Services;
using RequestHandler.Services.Kafka;
using RequestHandler.Services.KafkaProducers;
using RequestHandler.Services.MessageHandlers;

namespace RequestHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

            builder.Services.AddScoped<IRequestsConsumerService, KafkaRequestsConsumerService>();
            builder.Services.AddScoped<IMessageHandler, MessageHandler>();
            builder.Services.AddScoped<IHandledRequestsProducer, KafkaHandledRequestsProducer>();
            builder.Services.AddScoped<IRequestsDb, RequestsRedisDB>();
            builder.Services.AddHostedService<KafkaHostedService>();

            builder.AddRedisConnetionMultiplexer();
            var app = builder.Build();

            app.MapGet("/", () => "Request Handler is started");

            app.Run();
        }
    }
}
