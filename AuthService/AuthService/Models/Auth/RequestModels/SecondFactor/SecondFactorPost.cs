namespace AuthMicroservice.Models.Auth.RequestModels.SecondFactor
{
    public class SecondFactorPost
    {
        public string? Code { get; set; }
        public bool Remember { get; set; }
    }
}
