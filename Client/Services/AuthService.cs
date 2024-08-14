using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
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

    public async Task<ApiResult<object>> Register(RegisterModel registerModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/accounts", registerModel);

        var passedResult = await result.Content.ReadFromJsonAsync<ApiResult>();

        return passedResult;
    }

    public async Task<ApiResult<string>> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            return await ErrorResultHelper.CreateErrorResult<string>(nameof(LoginModel.Email));
        }

        var loginResult = JsonSerializer.Deserialize<ApiResult<string>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (loginResult is null)
        {
            return await ErrorResultHelper.CreateErrorResult<string>(nameof(LoginModel.Email));
        }

        if (!response.IsSuccessStatusCode)
        {
            return loginResult;
        }

        var token = loginResult.Data;

        if (token == null)
        {
            return await ErrorResultHelper.CreateErrorResult<string>(nameof(LoginModel.Email));
        }

        await _localStorage.SetItemAsync("authToken", token);
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
