namespace NotificationService.ContractModels
{
    public record EmailRequestDto(string to,
        string subject,
        string body);

}
