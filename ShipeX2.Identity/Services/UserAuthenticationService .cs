using Microsoft.AspNetCore.Authentication.Cookies;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Helpers;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using System.Security.Claims;

namespace ShipeX2.Identity.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        public UserAuthenticationService ( ApplicationDbContext context )
        {
            _context = context;
        }

        private async Task<User> GetUser ( string username, string password )
        {
            var loginCred = _context.LoginCredentials.FirstOrDefault(u => u.UserId == username);
            if (loginCred == null)
                return null;

            string isPasswordValid = await AesOperatonHelper.Encrypt(password);
            if (string.IsNullOrEmpty(isPasswordValid))
                return null;

            var role = _context.UserRoles.Where(r => r.RoleId == loginCred.RoleId).Select(r => r.Role).FirstOrDefault();
            return new User
            {
                Username = loginCred.UserId,
                Role = role,
                UserId = loginCred.Id
            };
        }

        public async Task<(bool IsSuccess, string ErrorMessage, ClaimsPrincipal Principal)> AuthenticateUserAsync ( string username, string password )
        {
            var user = GetUser(username,password);

            if (user.IsCanceled)
            {
                return (false, "Invalid username or password", null);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Result.Username),
                new Claim(ClaimTypes.Role, user.Result.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Result.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            return (true, null, principal);
        }

        public (string Controller, string Action) GetRedirectRouteByRole ( string role ) =>
        role switch
        {
            "Admin" => ("User", "Index"),
            "Shipper" => ("User", "Index"),
            "Super Admin" => ("User", "Index"),
            _ => ("Account", "AccessDenied")
        };
    }
}
