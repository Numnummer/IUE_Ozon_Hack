using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthMicroservice.Abstractions.Services
{
    public interface ITokenService
    {
        string GetRandomString(int length);
        DateTime UnixTimeStampToDateTime(double unixTimeStamp);
        ClaimsPrincipal? ValidateJwtToken(string jwtToken, out SecurityToken securityToken);
        string GetSecurityAlgoritm();
        string GetSecurityAlgoritmSignature();
    }
}
