using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ShipeX2.Application.Wrappers
{
    public class CurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser ( IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetCurrentUserId ()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }

            throw new Exception("UserId claim is missing or invalid.");
        }

        public string GetCurrentUserName ()
        {
            var userName = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(userName))
            {
                return userName;
            }

            throw new Exception("Username claim is missing or invalid.");
        }

        public string GetCurrentUserRole ()
        {
            var userRole = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Role)?.Value;
            if (!string.IsNullOrEmpty(userRole))
            {
                return userRole;
            }
            throw new Exception("UserRole claim is missing or invalid.");
        }

        public List<string> GetCurrentUserRoles ()
        {
            return _httpContextAccessor.HttpContext?.User?
                .Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }


    }

}
