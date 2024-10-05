namespace RequestHandler.Application
{
    public interface IConsumerService
    {
        void StartRequestsConsuming(CancellationToken cancellationToken);
        void StartResponsesConsuming(CancellationToken cancellationToken);
    }
}
