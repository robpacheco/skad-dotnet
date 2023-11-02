using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Skad.Common.Auth;

public static class AuthExtensions
{
    public static void EnableAuth(this IServiceCollection services, string loginUrl, string redisUrl)
    {
        var cookieName = "skad-auth";
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Cookie.Name = cookieName;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.Path = "/";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.Events = new CustomCookieAuthenticationEvents(loginUrl);
            options.LoginPath = "/login";
        });
        
        var redis = ConnectionMultiplexer.Connect(redisUrl);
        services.AddDataProtection()
            .SetApplicationName("skad")
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
    }
}