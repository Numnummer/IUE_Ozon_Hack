namespace AuthMicroservice.Models.Auth.ResponseModels
{
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
