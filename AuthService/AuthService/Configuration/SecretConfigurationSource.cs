namespace AuthMicroservice.Configuration
{
    public class SecretConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecretConfigurationProvider();
        }
    }
}
