using AuthMicroservice.Abstractions.UseCases;
using AuthMicroservice.Models.Auth.RequestModels.SecondFactor;
using AuthMicroservice.Models.Auth.RequestModels.UserData;
using AuthMicroservice.Models.Auth;
using AuthMicroservice.Options;
using AuthMicroservice.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NotificationService.ContractModels;

namespace AuthMicroservice.Models.User
{
    public class AppUserUseCases(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IAuthTokensUseCases authTokensUseCases,
        IPublishEndpoint publishEndpoint,
        ILogger<AppUserUseCases>? logger = null) : IAppUserUseCases
    {
        public async Task<AuthResult?> RegistrateUser(RegistrationUserData registrationUserData)
        {
            var currentUser = new AppUser()
            {
                Email = registrationUserData.Email,
                UserName=registrationUserData.FullName,
            };
            logger?.LogInformation("Создаем пользователя");
            var result = await userManager.CreateAsync(currentUser, registrationUserData.Password);
            logger?.LogInformation($"Результат: {result}");
            if (result.Succeeded)
            {
                var authResult = await signInManager
                    .CheckPasswordSignInAsync(currentUser, registrationUserData.Password, true);
                logger?.LogInformation(authResult.ToString());
                if (authResult.Succeeded)
                {
                    return await authTokensUseCases
                        .GenerateJwtAndRefreshTokensAsync(currentUser);
                }
                if (authResult.IsNotAllowed && !currentUser.EmailConfirmed)
                {
                    return new AuthResult()
                    {
                        NeedTwoFactor = true,
                        UserId = await userManager.GetUserIdAsync(currentUser)
                    };
                }
            }
            var userToDelete = await userManager.FindByEmailAsync(registrationUserData.Email);
            await userManager.DeleteAsync(userToDelete);
            return null;
        }

        public async Task SendEmailCodeAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var publishTask = publishEndpoint.Publish(
                    new EmailRequestDto(email, "Confirm your identity", token));
            var timeoutMilliseconds = 2000;
            if (await Task.WhenAny(publishTask, Task.Delay(timeoutMilliseconds))==publishTask)
            {
                logger?.LogInformation($"Сообщение для {email} отправлено сервису email");
            }
            else
            {
                logger?.LogError("Брокер сообщений не отвечает");
                throw new Exception("Брокер сообщений не отвечает");
            }
        }

        public async Task<AuthResult?> SecondFactorSignInAsync(SecondFactorPost data)
        {
            var user = await userManager.FindByIdAsync(data.UserId);
            if (user == null) return null;
            var result = await userManager.ConfirmEmailAsync(user, data.Code);
            if (result.Succeeded)
                return await authTokensUseCases.GenerateJwtAndRefreshTokensAsync(user);
            return null;
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded) return true;
            return false;
        }

        public async Task<AuthResult?> SignInUserAsync(SignInUserData signInUserData)
        {
            var currentUser = await userManager.FindByEmailAsync(signInUserData.Email);
            if (currentUser == null) return null;
            var authResult = await signInManager.CheckPasswordSignInAsync(currentUser, signInUserData.Password, true);
            if (authResult.Succeeded)
            {
                return await authTokensUseCases.GenerateJwtAndRefreshTokensAsync(currentUser);
            }
            if (authResult.IsNotAllowed && !currentUser.EmailConfirmed)
            {
                return new AuthResult()
                {
                    NeedTwoFactor = true,
                    UserId = await userManager.GetUserIdAsync(currentUser)
                };
            }
            return null;
        }
    }
}
