using Kwaliteitsprotocol_7B.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Kwaliteitsprotocol_7B.Services;

public sealed class AppAuthenticationStateProvider(AuthenticationService authenticationService) : AuthenticationStateProvider
{
    readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await authenticationService.GetUserSession() is { } userSession)
        {
            var claimsPrincipal = GetClaimsPrincipal(userSession);
            return new(claimsPrincipal);
        }

        return new(Anonymous);
    }

    public async Task UpdateAuthenticationState(UserSession? userSession, bool rememberUser = false)
    {
        if (userSession is not null)
        {
            if (rememberUser)
            {
                await authenticationService.SaveUserSession(userSession);
            }

            var claimsPrincipal = GetClaimsPrincipal(userSession);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        else
        {
            authenticationService.RemoveUserSession();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
        }
    }

    static ClaimsPrincipal GetClaimsPrincipal(UserSession userSession) => new(new ClaimsIdentity(
    [
        new(ClaimTypes.Name, userSession.Name),
    ], "AppAuth"));
}
