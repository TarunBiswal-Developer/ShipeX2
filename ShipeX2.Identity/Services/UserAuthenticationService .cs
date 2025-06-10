using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Helpers;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;
using ShipeX2.Identity.Context;
using System.Security.Claims;
using static ShipeX2.Persistence.TableModels.Tables;

namespace ShipeX2.Identity.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserAuthenticationService> _logger;
        private readonly CurrentUser _currentUser;

        public UserAuthenticationService ( ApplicationDbContext context, ILogger<UserAuthenticationService> logger, CurrentUser currentUser )
        {
            _context = context;
            _logger = logger;
            _currentUser = currentUser;
        }

        private async Task<User> GetUser ( string username, string password )
        {
            string isPasswordValid = await AesOperatonHelper.Encrypt(password);
            if (string.IsNullOrEmpty(isPasswordValid))
                return null;
            var loginCred = _context.LoginCredentials.FirstOrDefault(u => u.UserId == username && u.Password == isPasswordValid);
            if (loginCred == null)
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

        public async Task<ApiResult> CreateUserAsync ( UserModelExtended userModel )
        {
            ApiResult apiResult = new ApiResult();
            if (userModel == null)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = "User model cannot be null.";
                apiResult.Data = Array.Empty<string>();
                return apiResult;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var loginCredential = new LoginCredential
                {
                    UserId = userModel.UserId,
                    Password = await AesOperatonHelper.Encrypt(userModel.Password),
                    RoleId = userModel.RoleId,
                    Name = userModel.UserId,
                    Status = true,
                    CreatedBy = _currentUser.GetCurrentUserId(),
                    CreatedDate = DateTime.UtcNow
                };
                _context.LoginCredentials.Add(loginCredential);
                await _context.SaveChangesAsync();

                var shipXUser = new ShipXUser
                {
                    LabelPntId = userModel.LabelPntId,
                    InvoicePntId = userModel.InvoicePntId,
                    Id = loginCredential.Id,
                    Createdby = _currentUser.GetCurrentUserId(),
                    Createddate = DateTime.UtcNow
                };
                _context.ShipXUsers.Add(shipXUser);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                apiResult.IsSuccessful = true;
                apiResult.Message = "User inserted successfully!";
                apiResult.Data = loginCredential.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                apiResult.IsSuccessful = false;
                apiResult.Message = $"Error inserting user: {ex.Message}";
                apiResult.Data = Array.Empty<string>();
            }

            return apiResult;
        }


    }
}
