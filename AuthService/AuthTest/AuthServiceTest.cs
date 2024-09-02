using AuthMicroservice.Abstractions;
using AuthMicroservice.Abstractions.UseCases;
using AuthMicroservice.Models.Auth;
using AuthMicroservice.Models.Auth.RequestModels.UserData;
using AuthMicroservice.Models.User;
using AuthMicroservice.Options;
using AuthMicroservice.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;


namespace AuthTest
{
    [TestFixture]
    public class AuthServiceTest
    {

        [Test]
        public async Task RegistrateUser_Success()
        {
            //arrange
            var registrationData = new RegistrationUserData()
            {
                Email="binaryfile@mail.ru",
                FullName="Test Test",
                Role="Admin",
                Password="Password",
                Confirm="Password"
            };
            var user = new AppUser()
            {
                Email = registrationData.Email,
                UserName=registrationData.FullName,
            };
            var authResultStub = new AuthResult()
            {
                Token = "jwtToken",
                Success = true,
                RefreshToken = "refreshToken.Token"
            };
            var tokenServiceMock = new Mock<IAuthTokensUseCases>();
            tokenServiceMock.Setup(_ => _.GenerateJwtAndRefreshTokensAsync(It.IsAny<AppUser>()))
                .Returns(Task.FromResult(authResultStub));

            var publishEndpointMock = new Mock<IPublishEndpoint>();
            publishEndpointMock.Setup(_ => _.Publish(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var userManagerMock = AuthServiceTestHelper.GetUserManagerMock();
            userManagerMock.Setup(_ => _.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var signInManagerMock = AuthServiceTestHelper.GetSignInManagerMock(userManagerMock.Object);
            signInManagerMock.Setup(_ => _.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, true))
                .ReturnsAsync(SignInResult.Success);

            var monitor = Mock.Of<IOptionsMonitor<MailSettings>>(_ => _.CurrentValue == new MailSettings());

            var authService = new AppUserUseCases(userManagerMock.Object,
                signInManagerMock.Object, tokenServiceMock.Object,
                monitor, publishEndpointMock.Object);
            //act
            var result = await authService.RegistrateUser(registrationData);
            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
    }
}
