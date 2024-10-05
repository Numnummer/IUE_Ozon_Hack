using RequestHandler.Application;
using RequestHandler.Models;

namespace RequestHandler.Services.MessageHandlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IProduceService _producer;
        private readonly IRequestsDb _requestsDb;

        public MessageHandler(IProduceService producer, 
            IRequestsDb requestsDb)
        {
            _producer = producer;
            _requestsDb = requestsDb;
        }

        public void HandleMessage(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                throw new ArgumentException("Empty request string");
            }
            if (string.IsNullOrEmpty(product.RedirectUrl))
            {
                throw new ArgumentException("Empty redirect url");
            }

            if (!_requestsDb.IsRequestProcessing(product.Name.ToLower()))
            {
                _producer.ProduceHandledRequest(product.Name.ToLower());
                Console.WriteLine("Send Message to ML");
            }

            _requestsDb.AddNewRequest(product.Name, product.RedirectUrl);
            Console.WriteLine("Add new request to Redis");
            //_logger.LogInformation($"Request '{product.Name}' is in process, add {product.RedirectUrl} to waiting lists");
        }
    }
}
