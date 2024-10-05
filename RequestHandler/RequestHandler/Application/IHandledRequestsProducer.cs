namespace RequestHandler.Application
{
    public interface IHandledRequestsProducer
    {
        Task Produce(string mes);
    }
}
