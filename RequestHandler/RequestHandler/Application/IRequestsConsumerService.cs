namespace RequestHandler.Application
{
    public interface IRequestsConsumerService
    {
        void StartConsuming(CancellationToken cancellationToken);
    }
}
