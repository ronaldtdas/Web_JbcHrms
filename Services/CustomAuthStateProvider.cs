using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Web_JbcHrms.Models;

namespace Web_JbcHrms.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient, IConfiguration configuration)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await _localStorage.GetItemAsync<UserSession>("userSession");
            if (userSession == null)
                return new AuthenticationState(_anonymous);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Role, userSession.Role)
            }, "CustomAuth"));

            return new AuthenticationState(claimsPrincipal);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task<string> UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            if (string.IsNullOrWhiteSpace(userSession.Role) && !string.IsNullOrWhiteSpace(userSession.Uid))
            {
                userSession.Role = await FetchUserRole(userSession.Uid);
            }

            if (string.IsNullOrWhiteSpace(userSession.Role))
            {
                userSession.Role = "User";
            }

            await _localStorage.SetItemAsync("userSession", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Role, userSession.Role)
            }, "CustomAuth"));
        }
        else
        {
            await _localStorage.RemoveItemAsync("userSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        return userSession?.Role ?? "";
    }

    private async Task<string> FetchUserRole(string uid)
    {
        try
        {
            var firebaseConfig = _configuration.GetSection("FirebaseConfig").Get<FirebaseConfigOptions>();
            if (firebaseConfig == null || string.IsNullOrWhiteSpace(firebaseConfig.DatabaseURL) || string.IsNullOrWhiteSpace(firebaseConfig.ApiKey))
                return "User";

            var url = $"{firebaseConfig.DatabaseURL}/roles/admin/{uid}.json?auth={firebaseConfig.ApiKey}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var isAdmin = await response.Content.ReadFromJsonAsync<bool>();
                if (isAdmin)
                    return "Admin";
            }
        }
        catch { }

        return "User";
    }
}
