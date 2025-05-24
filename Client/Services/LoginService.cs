using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Client.Helpers;
using Client.ServiceContracts;
using Microsoft.AspNetCore.Components.Authorization;
using OneOf;
using Shared.Common;
using Shared.Login;

namespace Client.Services;

public class LoginService : ILoginService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public LoginService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<OneOf<string, ApiErrorResult>> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            {
                var loginResult = await response.Content.ReadAsStringAsync();
                await _localStorage.SetItemAsync("authToken", loginResult);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult);

                return loginResult;
            }
            case HttpStatusCode.BadRequest:
                return await response.Content.ReadFromJsonAsync<ApiErrorResult>() ?? ErrorResultHelper.CreateGeneralError();
            default:
                return ErrorResultHelper.CreateGeneralError();
        }
    }
}
