using AuthMicroservice.Abstractions;
using AuthMicroservice.Abstractions.UseCases;
using AuthMicroservice.Models.Auth.RequestModels.SecondFactor;
using AuthMicroservice.Models.Auth.RequestModels.UserData;
using AuthMicroservice.Models.Auth.ResponseModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserController(IAppUserUseCases appUserUseCases,
        ILogger<UserController> logger, IMapper mapper) : ControllerBase
    {
        /// <summary>
        /// Зарегистрироваться
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegistrateUser(RegistrationUserData registrationUserData)
        {
            logger.LogInformation("Начинаем процесс регистрации");
            var result = await appUserUseCases.RegistrateUser(registrationUserData);
            if (result==null)
            {
                logger.LogError("Не удалось зарегистрировать пользователя");
                return BadRequest("Не удалось зарегистрировать пользователя");
            }
            if (result.Success)
            {
                logger.LogInformation("Пользователь успешно зарегистрировался");
                return Ok(mapper.Map<AuthResponse>(result));
            }
            if (result.NeedTwoFactor)
            {
                logger.LogInformation("Необходимо подтверждение почты");
                return RedirectToAction("SecondFactorEmail", new { email = registrationUserData.Email });
            }
            return BadRequest();
        }

        /// <summary>
        /// Войти
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost("enter")]
        public async Task<IActionResult> SignInUser(SignInUserData signInUserData)
        {
            var result = await appUserUseCases.SignInUserAsync(signInUserData);
            if (result==null)
            {
                logger.LogError("Не удалось войти");
                return BadRequest("Не удалось войти");
            }
            if (result.Success)
            {
                logger.LogInformation("Пользователь успешно прошел в систему");
                return Ok(mapper.Map<AuthResponse>(result));
            }
            if (result.NeedTwoFactor)
            {
                logger.LogInformation("Необходимо подтверждение почты");
                return RedirectToAction("SecondFactorEmail", new { email = signInUserData.Email });
            }
            return BadRequest();
        }

        [HttpGet("secondFactor/{email}")]
        public async Task<IActionResult> SecondFactorEmail(string email)
        {
            await appUserUseCases.SendEmailCodeAsync(email);
            logger.LogInformation($"Код подтверждения отправлен сервису нотификации");
            return Ok("Код отправлен");
        }

        [HttpPost("secondFactor")]
        public async Task<IActionResult> SecondFactorEmail(SecondFactorPost postData)
        {
            var result = await appUserUseCases.SecondFactorSignInAsync(postData);
            if (result!=null)
            {
                logger.LogInformation("Пользователь аутентифицирован");
                return Ok(mapper.Map<AuthResponse>(result));
            }
            return StatusCode(500, "Не удалось авторизоваться");
        }

        [HttpDelete("deleteUser/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await appUserUseCases.DeleteUserAsync(email);
            if (result) return Ok();
            return StatusCode(500);
        }
    }
}