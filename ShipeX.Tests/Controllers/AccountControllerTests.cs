using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShipeX2.Web.Controllers;
using ShipeX2.Application.Interfaces;

namespace ShipeX.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public void Login_WhenUserIsAuthenticated_RedirectsBasedOnRole ()
        {
            // Arrange
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var controller = new AccountController(mockAuthService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            mockAuthService.Setup(s => s.GetRedirectRouteByRole("Admin"))
                           .Returns((Action: "Index", Controller: "User"));

            // Act
            var result = controller.Login() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User", result.ControllerName);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("ABCDEFGH", result.RouteValues ["clientId"]);
        }


        [Fact]
        public void Login_WhenUserIsNotAuthenticated_ReturnsLoginView ()
        {
            // Arrange
            var mockAuthService = new Mock<IUserAuthenticationService>();
            var controller = new AccountController(mockAuthService.Object);
            var emptyUser = new ClaimsPrincipal(new ClaimsIdentity());

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = emptyUser }
            };

            // Act
            var result = controller.Login();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }
    }

}
