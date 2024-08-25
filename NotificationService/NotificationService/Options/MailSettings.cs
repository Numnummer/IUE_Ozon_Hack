using Microsoft.Extensions.Options;

namespace NotificationService.Options
{
    public class MailSettings : IOptions<MailSettings>
    {
        public string? Sender { get; set; }
        public string? Server { get; set; }
        public int Port { get; set; }
        public string? Password { get; set; }
        public MailSettings Value => this;
    }
}
