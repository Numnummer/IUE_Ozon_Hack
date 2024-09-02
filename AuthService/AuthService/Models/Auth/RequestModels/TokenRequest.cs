using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models.Auth.RequestModels
{
    public class TokenRequest
    {
        [Required]
        public string JwtToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
