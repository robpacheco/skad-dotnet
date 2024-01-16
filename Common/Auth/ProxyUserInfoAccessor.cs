using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Skad.Common.Auth;

public class ProxyUserInfoAccessor : IUserInfoAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private OauthSettings _proxySettings;
    private ILogger<ProxyUserInfoAccessor> _logger;

    public ProxyUserInfoAccessor(IHttpContextAccessor httpContextAccessor, IOptions<OauthSettings> proxySettings, ILogger<ProxyUserInfoAccessor> logger)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _proxySettings = proxySettings.Value ?? throw new ArgumentNullException(nameof(proxySettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<UserInfo> FetchUserInfo()
    {
        var authCookie = _httpContextAccessor.HttpContext.Request.Cookies[_proxySettings.CookieName];
        var baseAddress = new Uri(_proxySettings.BaseUrl);

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(baseAddress, new Cookie(_proxySettings.CookieName, authCookie));

        var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        var client = new HttpClient(handler) { BaseAddress = baseAddress };
        
        var response = await client.GetAsync("/oauth2/userinfo");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Error getting user info. Status: {response.StatusCode}. Message: {content}");
            return new UserInfo
            {
                User = "Unknown"
            };
        }
        
        var userInfo = 
            JsonSerializer.Deserialize<UserInfo>(content);
        
        return userInfo ?? new UserInfo
        {
            User = "Unknown"
        };
    }
}