using RequestHandler.Application;
using RequestHandler.Services.Kafka;
using RequestHandler.Services.MessageHandlers;

namespace RequestHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

            builder.Services.AddScoped<IConsumerService, KafkaConsumerService>();
            builder.Services.AddScoped<IMessageHandler, MessageHandler>();
            builder.Services.AddHostedService<KafkaHostedService>();        

            var app = builder.Build();

            app.MapGet("/", () => "Request Handler is started");

            app.Run();
        }
    }
}
