using Microsoft.Extensions.Options;

namespace AuthMicroservice.Options
{
    public class JwtSettings : IOptions<JwtSettings>
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpireTimeInMinutes { get; set; }

        JwtSettings IOptions<JwtSettings>.Value => this;
    }

}
