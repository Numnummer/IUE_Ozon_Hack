using AuthMicroservice.Abstractions;
using AuthMicroservice.Abstractions.Repository;
using AuthMicroservice.Abstractions.Services;
using AuthMicroservice.Abstractions.UseCases;
using AuthMicroservice.Database.Repository;
using AuthMicroservice.Models.Auth.AuthTokens;
using AuthMicroservice.Models.User;
using AuthMicroservice.Services;

namespace AuthMicroservice.Extentions
{
    public static class WebApplicationBuilderExtentions
    {
        public static void RegisterAppServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ITokenService, TokenService>();
        }
        public static void RegisterAppUseCases(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAppUserUseCases, AppUserUseCases>();
            builder.Services.AddScoped<IAuthTokensUseCases, AuthTokensUseCases>();
        }
        public static void RegisterAppRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }

    }
}
