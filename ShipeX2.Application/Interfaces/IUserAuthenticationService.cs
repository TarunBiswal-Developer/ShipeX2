using System.Security.Claims;

namespace ShipeX2.Application.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<(bool IsSuccess, string ErrorMessage, ClaimsPrincipal Principal)> AuthenticateUserAsync ( string username, string password );
        (string Controller, string Action) GetRedirectRouteByRole(string role);
    }

}
