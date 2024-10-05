namespace RequestHandler.Application
{
    public interface IProduceService
    {
        Task ProduceHandledRequest(string request);
    }
}
