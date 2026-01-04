using Kwaliteitsprotocol_7B.Models;
using System.Text.Json;

namespace Kwaliteitsprotocol_7B.Services;

public sealed class AuthenticationService
{
    const string UserSessionKey = "app_user_session";

    public async Task<UserSession?> GetUserSession()
    {
        if (await SecureStorage.Default.GetAsync(UserSessionKey) is { Length: > 0 } userSessionJson)
        {
            return JsonSerializer.Deserialize<UserSession>(userSessionJson);
        }

        return null;
    }

    public void RemoveUserSession()
        => SecureStorage.Default.Remove(UserSessionKey);

    public async Task SaveUserSession(UserSession userSession)
    {
        var userSessionJson = JsonSerializer.Serialize(userSession);
        await SecureStorage.Default.SetAsync(UserSessionKey, userSessionJson);
    }
}
