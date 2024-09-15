using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using OneOf;
using Shared.Common;
using Shared.Login;
using Shared.Register;

namespace Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<OneOf<bool, List<ApiError>>> Register(RegisterModel registerModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/accounts", registerModel);

        return response.StatusCode switch
        {
            HttpStatusCode.OK => await response.Content.ReadFromJsonAsync<bool>(),
            HttpStatusCode.BadRequest => await response.Content.ReadFromJsonAsync<List<ApiError>>() ?? [],
            _ => ErrorResultHelper.CreateGeneralError()
        };
    }

    public async Task<OneOf<string, List<ApiError>>> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                var loginResult = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync())!;

                await _localStorage.SetItemAsync("authToken", loginResult);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult);

                return loginResult;
            }
            case HttpStatusCode.BadRequest:
                return await response.Content.ReadFromJsonAsync<List<ApiError>>() ?? [];
            default:
                return ErrorResultHelper.CreateGeneralError();
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
