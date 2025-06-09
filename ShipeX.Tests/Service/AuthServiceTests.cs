using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthServiceTests> _logger;
        private readonly ApplicationDbContext _context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());

        public AuthServiceTests ( IUserAuthenticationService userAuthenticationService,ILogger<AuthServiceTests> logger,ApplicationDbContext context  )
        {
            _context = context;
            _authService = userAuthenticationService;
            _logger = logger;
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
