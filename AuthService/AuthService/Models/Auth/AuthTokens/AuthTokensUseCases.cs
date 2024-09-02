using AuthMicroservice.Abstractions.Repository;
using AuthMicroservice.Abstractions.Services;
using AuthMicroservice.Abstractions.UseCases;
using AuthMicroservice.Database;
using AuthMicroservice.Extentions;
using AuthMicroservice.Models.Auth.RequestModels;
using AuthMicroservice.Models.User;
using AuthMicroservice.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthMicroservice.Models.Auth.AuthTokens
{
    public class AuthTokensUseCases(IConfiguration configuration,
        IOptionsMonitor<JwtSettings> optionsMonitor,
        TokenValidationParameters tokenValidationParameters,
        UserManager<AppUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService) : IAuthTokensUseCases
    {
        /// <summary>
        /// Создаёт новый jwt токен и рефреш токен для
        /// заданного пользователя
        /// </summary>
        /// <param name="user">Пользователь приложения</param>
        /// <returns></returns>
        public async Task<AuthResult> GenerateJwtAndRefreshTokensAsync(AppUser user)
        {
            var jwtTokenId = Guid.NewGuid().ToString();
            var jwtToken = GenerateJwtToken(user, jwtTokenId);

            var refreshToken = new RefreshToken()
            {
                JwtId = jwtTokenId,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = tokenService.GetRandomString(25) + Guid.NewGuid()
            };

            await refreshTokenRepository.AddAsync(refreshToken);

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        /// <summary>
        /// По действующему рефреш токену и устаревшему
        /// jwt токену возвращает новый jwt токен
        /// </summary>
        /// <param name="tokenRequest">Jwt и рефреш токен</param>
        /// <returns></returns>
        public async Task<AuthResult?> RefreshJwtTokenAsync(TokenRequest tokenRequest)
        {
            try
            {
                //Проверка, является ли пришедший jwt токен валидным
                var principal = tokenService.ValidateJwtToken(tokenRequest.JwtToken,
                    out var validatedToken);

                //Проверка, устарел ли пришедший jwt токен
                //если устарел то идем дальше
                //если нет то возвращаем результат с ошибкой
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expDate = tokenService.UnixTimeStampToDateTime(utcExpiryDate);
                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Success = false
                    };
                }

                // Проверка, есть ли в нашей базе полученный рефреш токен
                var storedRefreshToken = await refreshTokenRepository.GetRefreshTokenByTokenStringAsync(tokenRequest.RefreshToken);
                if (storedRefreshToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Success = false
                    };
                }

                // Проверка, не устарел ли полученный рефреш токен
                //если устарел то удаляем его из базы и
                //возвращаем сообщение с ошибкой
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    await refreshTokenRepository.DeleteRefreshTokenByTokenStringAsync(storedRefreshToken.Token);
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Success = false
                    };
                }

                // Получаем id полученного jwt токена
                var jwtId = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // Проверка связан ли просроченный jwt токен которого мы получили
                // с полученным рефреш токеном
                if (storedRefreshToken.JwtId != jwtId)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Success = false
                    };
                }

                // Проверка есть ли в базе пользователь с id, указанный в 
                // рефреш токене
                var dbUser = await userManager.FindByIdAsync(storedRefreshToken.UserId);
                if (dbUser == null) return null;

                return GenerateJwtTokenByRefreshToken(dbUser, storedRefreshToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GenerateJwtToken(AppUser user, string tokenId)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(optionsMonitor.CurrentValue.Key);
            var tokenDescriptor = CreateTokenDescriptor(user, tokenId, key);
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor? CreateTokenDescriptor(AppUser user, string tokenId, byte[] key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenId)
                }),
                Expires = DateTime.UtcNow
                    .AddMinutes(optionsMonitor.CurrentValue.ExpireTimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    tokenService.GetSecurityAlgoritmSignature())
            };
        }

        private AuthResult GenerateJwtTokenByRefreshToken(AppUser user, RefreshToken refreshToken)
        {
            var jwtToken = GenerateJwtToken(user, refreshToken.JwtId);
            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
