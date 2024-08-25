using MassTransit;
using NotificationService.Abstractions;
using NotificationService.ContractModels;

namespace NotificationService.MessageBroker
{
    public class EmailConsumer(IEmailService emailService,
        ILogger<EmailConsumer> logger) :
        IConsumer<EmailRequestDto>
    {
        public async Task Consume(ConsumeContext<EmailRequestDto> context)
        {
            await emailService.SendEmailAsync(context.Message);
        }
    }
}
