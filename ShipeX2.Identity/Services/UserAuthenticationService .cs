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

        private async Task<User?> GetUser ( string username, string password )
        {
            string isPasswordValid = await AesOperatonHelper.Encrypt(password);
            if (string.IsNullOrEmpty(isPasswordValid))
                return null;

            var loginCred = await _context.LoginCredentials.FirstOrDefaultAsync(u => u.UserId == username && u.Password == isPasswordValid);

            if (loginCred == null)
                return null;

            string statusMessage = loginCred.Status.Value ? "Active" : "Inactive";

            var role = await _context.UserRoles.Where(r => r.RoleId == loginCred.RoleId).Select(r => r.Role).FirstOrDefaultAsync();
            var userDetails = new
            {
                Username = loginCred.UserId,
                Role = role,
                UserId = loginCred.Id,
                StatusMessage = statusMessage
            };

            return userDetails.StatusMessage == "Inactive" ? null : new User
            {
                Username = userDetails.Username,
                Role = userDetails.Role,
                UserId = userDetails.UserId
            };
        }


        public async Task<(bool IsSuccess, string ErrorMessage, ClaimsPrincipal Principal)> AuthenticateUserAsync ( string username, string password )
        {
            var user = GetUser(username, password);
            if (user.Result == null)
            {
                return (false, "Invalid username or password | User Is Inactive", null);
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
            "Admin" => ("ShipmentHistory", "Index"),
            "Shipper" => ("User", "Index"),
            "Super Admin" => ("ShipmentHistory", "Index"),
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
                                        Status = uc.Status.Value
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
                apiResult.Message = $"Error inserting user: {ex.InnerException.Message}";
                apiResult.Data = Array.Empty<string>();
            }

            return apiResult;
        }

        public async Task<ApiResult> ActiveInactiveUser ( long userId )
        {
            ApiResult apiResult = new ApiResult();
            try
            {
                var user = await _context.LoginCredentials.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.Status = !user.Status.Value;
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        apiResult.IsSuccessful = true;
                        apiResult.Message = user.Status.Value ? "User activated successfully!" : "User deactivated successfully!";
                    }
                }
                else
                {
                    apiResult.IsSuccessful = false;
                    apiResult.Message = "User not found.";
                }
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessful = false;
                apiResult.Message = $"Error updating user status: {ex.Message}";
            }
            return apiResult;

        }

        public async Task<ModelUser> GetIdWiseUserList ( long id )
        {
            try
            {
                var encryptedUser = await _context.LoginCredentials.Where(uc => uc.Id == id).Join(_context.UserRoles,uc => uc.RoleId,ur => ur.RoleId, ( uc, ur ) => new { uc, ur })
                                    .Join(_context.ShipXUsers, combined => combined.uc.Id, su => su.Id, ( combined, su ) => new
                                        {
                                            Id = (int) combined.uc.Id,
                                            UserId = combined.uc.UserId,
                                            Role = combined.ur.Role,
                                            RoleId = (int) combined.uc.RoleId,
                                            Name = combined.uc.Name,
                                            EncryptedPassword = combined.uc.Password,
                                            Status = combined.uc.Status.Value,
                                            LabelPntId = (int) su.LabelPntId,
                                            InvoicePntId = (int) su.InvoicePntId,
                                        })
                                    .OrderByDescending(vm => vm.Id)
                                    .FirstOrDefaultAsync();

                if (encryptedUser != null)
                {
                    var decryptedPassword = await AesOperatonHelper.Decrypt(encryptedUser.EncryptedPassword);
                    var user = new ModelUser
                    {
                        Id = encryptedUser.Id,
                        UserId = encryptedUser.UserId,
                        Role = encryptedUser.Role,
                        RoleId = encryptedUser.RoleId,
                        Name = encryptedUser.Name,
                        Password = decryptedPassword,
                        Status = encryptedUser.Status,
                        LabelPntId = encryptedUser.LabelPntId,
                        InvoicePntId = encryptedUser.InvoicePntId,
                    };
                    return user;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user by ID.");
                return null;
                throw;
            }
        }

        public async Task<ApiResult> SaveUserDetails ( UserModelExtended model )
        {
            ApiResult apiResult = new ApiResult();
            using var transaction = await _context.Database.BeginTransactionAsync();
            DateTime currentDate = DateTime.UtcNow.Date;
            string formattedDate = currentDate.ToString("yyyy-MM-dd");

            try
            {
                if (model == null)
                {
                    apiResult.Message = "User model cannot be null.";
                    apiResult.IsSuccessful = false;
                    return apiResult;
                }
                if (long.TryParse(model.Id, out long userId))
                {
                    var existingUser = await _context.LoginCredentials.FirstOrDefaultAsync(uc => uc.Id == userId);
                    if (existingUser != null)
                    {
                        existingUser.UserId = model.UserId;
                        existingUser.Password = await AesOperatonHelper.Encrypt(model.Password);
                        existingUser.RoleId = model.RoleId;
                        existingUser.Name = model.UserId;
                        existingUser.UpdatedBy = _currentUser.GetCurrentUserId();
                        existingUser.UpdatedDate =  Convert.ToDateTime(formattedDate);

                        _context.LoginCredentials.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }
                }

                // Update ShipXUser details
                if (long.TryParse(model.Id, out long userIds))
                {
                    var existingShipXUser = await _context.ShipXUsers.FirstOrDefaultAsync(su => su.Id == userIds);
                    if (existingShipXUser != null)
                    {
                        existingShipXUser.LabelPntId = model.LabelPntId;
                        existingShipXUser.InvoicePntId = model.InvoicePntId;
                        existingShipXUser.Modifiedby = _currentUser.GetCurrentUserId();
                        existingShipXUser.Modifieddate = DateTime.UtcNow;

                        _context.ShipXUsers.Update(existingShipXUser);
                        await _context.SaveChangesAsync();
                    }
                }

                apiResult.Message = "User details updated successfully.";
                apiResult.IsSuccessful = true;
                apiResult.Data = model.Id;
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating user details.");
                apiResult.Message = $"Error updating user details: {ex.InnerException.Message}";
                apiResult.IsSuccessful = false;
            }

            return apiResult;
        }

    }
}
