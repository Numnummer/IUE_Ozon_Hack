using AuthMicroservice.Abstractions.Repository;
using AuthMicroservice.Models.Auth.AuthTokens;
using AuthMicroservice.Models.Auth.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Database.Repository
{
    public class RefreshTokenRepository(UserDbContext userDbContext) :
        IRefreshTokenRepository
    {
        public async Task AddAsync(RefreshToken refreshToken)
        {
            await userDbContext.RefreshTokens.AddAsync(refreshToken);
            await userDbContext.SaveChangesAsync();
        }

        public async Task DeleteRefreshTokenByTokenStringAsync(string token)
        {
            var tokenObj = await userDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            userDbContext.RefreshTokens.Remove(tokenObj);
            await userDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenStringAsync(string token)
        {
            return await userDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
