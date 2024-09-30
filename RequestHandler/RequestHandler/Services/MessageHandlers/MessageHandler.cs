using RequestHandler.Application;
using RequestHandler.Models;

namespace RequestHandler.Services.MessageHandlers
{
    public class MessageHandler : IMessageHandler
    {
        public void HandleMessage(Product product)
        {
            Console.WriteLine("Handle request...");
        }
    }
}
