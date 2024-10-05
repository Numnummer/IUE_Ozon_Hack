using RequestHandler.Models;

namespace RequestHandler.Application
{
    public interface IMessageHandler
    {
        void HandleMessage(Product product);
    }
}
