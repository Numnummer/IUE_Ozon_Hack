using AuthMicroservice.Models.Auth.RequestModels.SecondFactor;
using AuthMicroservice.Models.Auth.RequestModels.UserData;
using AuthMicroservice.Models.Auth;

namespace AuthMicroservice.Abstractions.UseCases
{
    public interface IAppUserUseCases
    {
        Task<AuthResult?> RegistrateUser(RegistrationUserData registrationUserData);
        Task SendEmailCodeAsync(string email);
        Task<AuthResult?> SecondFactorSignInAsync(SecondFactorPost data);
        Task<bool> DeleteUserAsync(string email);
        Task<AuthResult?> SignInUserAsync(SignInUserData signInUserData);
    }
}
