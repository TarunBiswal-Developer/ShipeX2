using Microsoft.AspNetCore.Builder;

namespace Shipex.Web.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandler ( this IApplicationBuilder app )
        {
            return app.UseMiddleware<WebSocketMiddleware>();
        }
    }
}
