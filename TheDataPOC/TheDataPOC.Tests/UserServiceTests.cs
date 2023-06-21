namespace TheDataPOC.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Application.Services;
    using Application.Services.Interfaces;
    
    using Domain.Models;
    
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Identity;
    
    using Moq;

    public class UserServiceTests
    {
        private const string AdminRoleName = "Admin";

        private readonly Mock<UserManager<User>> userManagerMock;

        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly Mock<RoleManager<IdentityRole>> roleManagerMock;

        private readonly IUserService userService;

        public UserServiceTests()
        {
            userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            unitOfWorkMock = new Mock<IUnitOfWork>();

            roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            userService = new UserService(userManagerMock.Object, unitOfWorkMock.Object, roleManagerMock.Object);
        }


        [Fact]
        public async Task GetUsers_ReturnsUsers()
        {
            // Arrange
            var users = new List<User> { new User(), new User() };
            unitOfWorkMock.Setup(u => u.GetRepository<User>().Get(It.IsAny<Expression<Func<User, bool>>>()))
               .Returns(users.AsQueryable());

            // Act
            var result = await userService.GetAsync();

            // Assert
            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task DeleteUser_DeletesUser()
        {
            // Arrange
            var user = new User();
            userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userService.DeleteUser(user);

            // Assert
            Assert.True(result.Succeeded);
            userManagerMock.Verify(x => x.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNullIfUserNotFound()
        {
            // Arrange
            User user = null;

            // Act
            var result = await userService.DeleteUser(user);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveRoleFromUser_RemovesRole()
        {
            // Arrange
            var user = new User();

            userManagerMock.Setup(u => u.RemoveFromRoleAsync(user, AdminRoleName))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userService.RemoveRoleFromUser(user, AdminRoleName);

            // Assert
            Assert.True(result.Succeeded);

            userManagerMock.Verify(u => u.RemoveFromRoleAsync(user, AdminRoleName), Times.Once);
        }
    }
}
