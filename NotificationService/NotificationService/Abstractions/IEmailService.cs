
using NotificationService.ContractModels;

namespace NotificationService.Abstractions
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequestDto emailRequestDto);
    }
}
