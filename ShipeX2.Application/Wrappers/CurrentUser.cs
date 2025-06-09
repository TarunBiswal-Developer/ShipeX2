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
    }

}
