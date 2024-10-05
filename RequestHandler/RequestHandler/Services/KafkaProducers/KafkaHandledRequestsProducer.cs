using RequestHandler.Application;

namespace RequestHandler.Services.KafkaProducers
{
    public class KafkaHandledRequestsProducer : IHandledRequestsProducer
    {
        public Task Produce(string mes)
        {
            Console.WriteLine("It's not implemented yet. But we think it's okay");
            return Task.CompletedTask;
        }
    }
}
