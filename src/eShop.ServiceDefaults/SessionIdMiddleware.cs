using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace eShop.ServiceDefaults;

public class SessionIdMiddleware(RequestDelegate next)
{
    private const string SessionIdHeaderName = "X-Session-Id";
    private const string SessionIdCookieName = "eshop_session_id";
    private const string SessionIdActivityTagName = "session_id";
    private const string UserIdActivityTagName = "user_id";

    public async Task InvokeAsync(HttpContext context)
    {
        // Precedência: header (teste de carga) > cookie (browser) > novo
        var sessionId =
            context.Request.Headers.TryGetValue(SessionIdHeaderName, out var headerValue) && !string.IsNullOrEmpty(headerValue)
                ? headerValue.ToString()
            : context.Request.Cookies.TryGetValue(SessionIdCookieName, out var cookieValue) && !string.IsNullOrEmpty(cookieValue)
                ? cookieValue
                : null;

        if (string.IsNullOrEmpty(sessionId))
        {
            sessionId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append(SessionIdCookieName, sessionId, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = context.Request.IsHttps,
                MaxAge = TimeSpan.FromHours(2)
            });
        }

        context.Items[SessionIdHeaderName] = sessionId;
        Activity.Current?.SetTag(SessionIdActivityTagName, sessionId);

        await next(context);

        // user_id só é capturado após o pipeline porque depende do UseAuthentication
        var userId = context.User?.FindFirst("sub")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            Activity.Current?.SetTag(UserIdActivityTagName, userId);
        }
    }
}

public static class SessionIdExtensions
{
    public static WebApplication UseSessionId(this WebApplication app)
    {
        app.UseMiddleware<SessionIdMiddleware>();
        return app;
    }

    public static string? GetSessionId(this HttpContext context)
    {
        return context.Items.TryGetValue("X-Session-Id", out var sessionId)
            ? sessionId?.ToString()
            : null;
    }
}
