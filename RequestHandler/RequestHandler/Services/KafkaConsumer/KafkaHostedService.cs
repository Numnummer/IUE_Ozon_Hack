using RequestHandler.Application;

namespace RequestHandler.Services.Kafka
{
    public class KafkaHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var kafkaConsumerService = scope.ServiceProvider.GetRequiredService<IConsumerService>();
            kafkaConsumerService.StartConsuming(stoppingToken);
            return Task.CompletedTask;
        }
    }
}
