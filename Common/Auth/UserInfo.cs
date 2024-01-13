using System.Text.Json.Serialization;

namespace Skad.Common.Auth;

public class UserInfo
{
    [JsonPropertyName("user")]
    public string User { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
