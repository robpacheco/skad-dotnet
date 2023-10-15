using Microsoft.AspNetCore.Authentication.Cookies;

namespace Skad.Common.Auth;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    public CustomCookieAuthenticationEvents(string loginUrl)
    {
        var baseOnRedirectToLogin = OnRedirectToLogin;

        OnRedirectToLogin = (context) =>
        {
            context.RedirectUri = loginUrl;
            return baseOnRedirectToLogin(context);
        };
    }
}