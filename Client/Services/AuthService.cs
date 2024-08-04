﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Login;
using Shared.Register;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Shared.Common;

namespace Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<ApiResult> Register(RegisterModel registerModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/accounts", registerModel);

        var passedResult = await result.Content.ReadFromJsonAsync<ApiResult>();

        return passedResult;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/Login",
            new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
        var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (!response.IsSuccessStatusCode)
        {
            return loginResult;
        }

        await _localStorage.SetItemAsync("authToken", loginResult.Token);
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}