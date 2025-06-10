using ShipeX2.Application.DTOs;
using ShipeX2.Application.Wrappers;
using System.Security.Claims;

namespace ShipeX2.Application.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<(bool IsSuccess, string ErrorMessage, ClaimsPrincipal Principal)> AuthenticateUserAsync ( string username, string password );
        (string Controller, string Action) GetRedirectRouteByRole(string role);
        Task<List<ModelUser>> GetUserListAsync();
        Task<ApiResult> CreateUserAsync ( UserModelExtended userModel );

        Task<ApiResult> ActiveInactiveUser (long userId);

        Task<ModelUser> GetIdWiseUserList ( long id );
        Task<ApiResult> SaveUserDetails ( UserModelExtended model);

    }
}
