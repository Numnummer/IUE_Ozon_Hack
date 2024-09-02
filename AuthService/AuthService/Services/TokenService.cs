using AuthMicroservice.Abstractions.Services;
using AuthMicroservice.Models.Auth.RequestModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace AuthMicroservice.Services
{
    public class TokenService(TokenValidationParameters tokenValidationParameters)
        : ITokenService
    {
        public string GetRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public string GetSecurityAlgoritm()
        {
            return SecurityAlgorithms.HmacSha256;
        }

        public string GetSecurityAlgoritmSignature()
        {
            return SecurityAlgorithms.HmacSha256Signature;
        }

        public ClaimsPrincipal? ValidateJwtToken(string jwtToken, out SecurityToken securityToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var principal = jwtTokenHandler.ValidateToken(jwtToken,
                    tokenValidationParameters,
                    out securityToken);
            if (securityToken is JwtSecurityToken jwtSecurityToken
                && principal != null)
            {
                // Проверка, правильный ли алгоритм шифрования используется
                // в пришедшем токене
                var result = jwtSecurityToken.Header.Alg
                        .Equals(GetSecurityAlgoritm(),
                        StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    return null;
                }
            }
            return principal;
        }
    }
}
