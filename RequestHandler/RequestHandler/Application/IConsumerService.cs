namespace RequestHandler.Application
{
    public interface IConsumerService
    {
        void StartConsuming(CancellationToken cancellationToken);
    }
}
