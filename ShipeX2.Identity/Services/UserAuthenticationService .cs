using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserAuthenticationService> _logger;
        public UserAuthenticationService ( ApplicationDbContext context, ILogger<UserAuthenticationService> logger )
        {
            _context = context;
            _logger = logger;
        }

        private async Task<User> GetUser ( string username, string password )
        {
            string isPasswordValid = await AesOperatonHelper.Encrypt(password);
            var loginCred = _context.LoginCredentials.FirstOrDefault(u => u.UserId == username && u.Password == isPasswordValid);
            if (loginCred == null)
                return null;
            if (string.IsNullOrEmpty(isPasswordValid))
                return null;

            var role = await _context.UserRoles.Where(r => r.RoleId == loginCred.RoleId).Select(r => r.Role).FirstOrDefaultAsync();
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
            if (user.Result == null)
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
            "Admin" => ("User", "UserList"),
            "Shipper" => ("User", "Index"),
            "Super Admin" => ("User", "UserList"),
            _ => ("Account", "AccessDenied")
        };

        public async Task<List<ModelUser>> GetUserListAsync ()
        {
            List<ModelUser> userList = new List<ModelUser>();
            try
            {
                return await _context.LoginCredentials
                                .Where(uc => uc.RoleId != 3)
                                .Join(
                                    _context.UserRoles,
                                    uc => uc.RoleId,
                                    ur => ur.RoleId,
                                    ( uc, ur ) => new ModelUser
                                    {
                                        Id = (int) uc.Id,
                                        UserId = uc.UserId,
                                        Role = ur.Role,
                                        Name = uc.Name,
                                        Password = uc.Password,
                                        Status = uc.Status
                                    }
                                )
                                .OrderByDescending(vm => vm.Id)
                                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user list.");
                throw;
            }
        }
    }
}
