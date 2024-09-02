using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NotificationService.Abstractions;
using NotificationService.MessageBroker;
using NotificationService.Options;
using SendEmailService = NotificationService.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MailSettings>(options =>
    builder.Configuration.GetSection("Mail").Bind(options));
builder.Services.AddTransient<IEmailService, SendEmailService>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        config.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

app.MapGet("/", ([FromServices] IOptions<MailSettings> mailSettingsMonitor,
        [FromServices] ILogger<MailSettings> logger,
        [FromServices] IConfiguration configuration) =>
{
    logger.LogInformation(mailSettingsMonitor.Value.Password);
});

app.Run();
