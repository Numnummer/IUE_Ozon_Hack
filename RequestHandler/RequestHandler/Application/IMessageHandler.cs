using RequestHandler.Models;

namespace RequestHandler.Application
{
    public interface IMessageHandler
    {
        public void HandleMessage(Product product);
    }
}
