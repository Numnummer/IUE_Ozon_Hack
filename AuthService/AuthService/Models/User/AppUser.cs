using Microsoft.AspNetCore.Identity;

namespace AuthMicroservice.Models.User
{
    public class AppUser : IdentityUser
    {
        public bool IsEmailConfirmed { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
