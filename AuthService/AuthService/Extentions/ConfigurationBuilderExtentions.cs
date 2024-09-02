using AuthMicroservice.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Extentions
{
    public static class ConfigurationBuilderExtentions
    {
        public static IConfigurationBuilder AddSecrets(
               this IConfigurationBuilder builder)
        {
            return builder.Add(new SecretConfigurationSource());
        }
    }
}
