using AuthMicroservice.Models.Auth.AuthTokens;
using AuthMicroservice.Models.Auth.RequestModels;

namespace AuthMicroservice.Abstractions.Repository
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenByTokenStringAsync(string token);
        Task DeleteRefreshTokenByTokenStringAsync(string token);
    }
}
