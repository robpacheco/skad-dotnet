using System.Threading.Tasks;

namespace Skad.Common.Auth;

public interface IUserInfoAccessor
{
    Task<UserInfo> FetchUserInfo();
}

public class EmptyUserInfoAccessor : IUserInfoAccessor
{
    public async Task<UserInfo> FetchUserInfo()
    {
        return new UserInfo
        {
            User = "Unknown",
            Email = "unknown@example.org"
        };
    }
}