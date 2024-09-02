namespace AuthMicroservice.Models.Auth.RequestModels.SecondFactor
{
    public class SecondFactorGet
    {
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
