using Microsoft.EntityFrameworkCore;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using ShipeX2.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipeX.Tests.Service
{
    public class AuthServiceTests
    {
        private readonly IUserAuthenticationService _authService;
        private readonly ApplicationDbContext _context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());

        public AuthServiceTests ()
        {
            _authService = new UserAuthenticationService(_context);
        }

        [Theory]
        [InlineData("Admin", "User", "Index")]
        [InlineData("User", "User", "Index")]
        [InlineData("Manager", "Manager", "Dashboard")]
        [InlineData("UnknownRole", "Account", "AccessDenied")]
        public void GetRedirectRouteByRole_ShouldReturnCorrectControllerAndAction ( string role, string expectedController, string expectedAction )
        {
            // Act
            var result = _authService.GetRedirectRouteByRole(role);

            // Assert
            Assert.Equal(expectedController, result.Controller);
            Assert.Equal(expectedAction, result.Action);
        }
    }

}
