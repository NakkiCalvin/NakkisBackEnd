using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using API.Requests;
using API.Responses;
using BLL.Entities;
using BLL.Managers;
using BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace APITests.UserTests
{
    public class ControllerTests
    {
        private static Mock<IUserManager> _mockedManager = new Mock<IUserManager>();
        private static Mock<ISignInManager> _mockedSignInManager = new Mock<ISignInManager>();
        private static Mock<IRoleManager> _mockedRoleManager = new Mock<IRoleManager>();
        private static Mock<ITokenService> _mockedTokenManager = new Mock<ITokenService>();
        private static Mock<ILogger<AccountController>> _mockedLogger = new Mock<ILogger<AccountController>>();

        AccountController _controller = new AccountController(_mockedManager.Object, _mockedSignInManager.Object, _mockedRoleManager.Object, _mockedTokenManager.Object, _mockedLogger.Object);

        [Fact]
        public async Task CheckCreateUser()
        {
            var user = new User
            {
                Email = "aaaa@gmail.com",
            };

            _mockedManager.Setup(p => p.CreateUser(user, "Qqqqqqqq12_")).Returns(Task.FromResult(new IdentityResult()));

            var model = new RegisterUserModel { Email = "aaaa@gmail.com", Password = "Qqqqqqqq12_" };

            var result = await _controller.Register(model);

            Assert.IsAssignableFrom<ActionResult>(result);
            Assert.NotNull(result);
            _mockedManager.Verify(x => x.CreateUser(It.IsAny<User>(), It.Is<string>(pass => pass == "Qqqqqqqq12_")), Times.Once);
        }

        [Fact]
        public async Task CheckAddToRole()
        {
            var user = new User
            {
                Email = "aaaa@gmail.com",
            };

            var model = new RegisterUserModel { Email = user.Email, Password = "Qqqqqqqq12_" };

            var result = await _controller.Register(model);

            _mockedManager.Verify(x => x.AddToRole(user, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CheckGetUserRoles()
        {
            var user = new User
            {
                Email = "aaaa@gmail.com",
            };

            var model = new RegisterUserModel { Email = user.Email, Password = "Qqqqqqqq12_" };
            var result = await _controller.Register(model);
            _mockedManager.Setup(x => x.AddToRole(user, It.IsAny<string>())).Returns(Task.FromResult(new IdentityResult()));
            _mockedManager.Setup(p => p.GetUserRoles(user)).Returns(Task.FromResult(It.IsAny<IList<string>>()));

            _mockedManager.Verify(x => x.GetUserRoles(user), Times.Once);
        }

        [Fact]
        public async Task CheckUserByEmail()
        {
            _mockedManager.Setup(x => x.GetUserByEmail("vova@gmail.com")).Returns(Task.FromResult(new User()));

            var logmodel = new LoginModel { Email = "vova@gmail.com", Password = "fafafa1sfAAa_" };
            var res = _controller.GenerateToken(logmodel);

            _mockedTokenManager.Verify(x => x.GetEncodedJwtToken(logmodel.Email), Times.Once);
        }

        [Fact]
        public async Task Logout()
        {
            _mockedSignInManager.Setup(x => x.Logout()).Returns(Task.CompletedTask);
            await _controller.Logout();
            _mockedSignInManager.Verify(x => x.Logout(), Times.Once);
        }

        [Fact]
        public async Task CheckLogin()
        {
            LoginModel model = new LoginModel() { Email = "vovanss@gmail.com", Password = "Djdfy123_" };
            var result = _controller.GenerateToken(model);
            _mockedTokenManager.Verify(x => x.GetEncodedJwtToken(model.Email), Times.Once);
        }

    }
}