namespace TheDataPOC.Tests 
{
    using Application.Services;

    using Domain.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    
    using Moq;
    
    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(
                new Mock<IUserStore<User>>().Object,
                new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object)
        { }
    }

    public class FakeSignInManager : SignInManager<User>
    {            
        public FakeSignInManager()
            : base(
                new Mock<FakeUserManager>().Object,
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
                new Mock<Microsoft.AspNetCore.Identity.IUserConfirmation<User>>().Object
                )
        { }
    }
    
    public class AuthServiceTests
    {
        private  Mock<FakeSignInManager> signInManagerMock;
        private AccountService accountService;

        public AuthServiceTests() 
        {
            signInManagerMock = new Mock<FakeSignInManager>();
        }

        [Fact]
        public async Task ValidateCredentialsPositive() 
        {
            var user = new User()
            {
                UserName = "testUser"
            };

            signInManagerMock.Setup(x => x.PasswordSignInAsync(user,
             It.IsAny<string>(),
             It.IsAny<bool>(),
             It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);

            Assert.Equal(await signInManagerMock.Object.PasswordSignInAsync(user,
            "string1A!", false, false), SignInResult.Success);
        }

        [Fact]
        public async Task ValidateCredentialsNegative() 
        {
            var user = new User() 
            {
                UserName = "testUser"
            };

            signInManagerMock.Setup(x => x.PasswordSignInAsync(user,
             It.IsAny<string>(),
             It.IsAny<bool>(),
             It.IsAny<bool>())).ReturnsAsync(SignInResult.Failed);

            Assert.Equal(await signInManagerMock.Object.PasswordSignInAsync(user,
            "string1A!", false, false), SignInResult.Failed);
        }
    }
}