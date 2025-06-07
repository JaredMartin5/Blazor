using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Client.Helpers;
using Client.ServiceContracts;
using Microsoft.AspNetCore.Components.Authorization;
using OneOf;
using Shared.AccountUser;
using Shared.Common;
using Shared.Register;

namespace Client.Services;

public class AccountsService : IAccountsService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AccountsService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<OneOf<bool, ApiErrorResult>> Register(RegisterModel registerModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/accounts", registerModel);
        return response.StatusCode switch
        {
            HttpStatusCode.OK => await response.Content.ReadFromJsonAsync<bool>(),
            HttpStatusCode.BadRequest => await response.Content.ReadFromJsonAsync<ApiErrorResult>() ?? ErrorResultHelper.CreateGeneralError(),
            _ => ErrorResultHelper.CreateGeneralError()
        };
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<OneOf<List<AccountUserDto>, ApiErrorResult>> GetAllAccounts()
    {
        var response = await _httpClient.GetAsync("api/accounts/table");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<AccountUserDto>>() ?? [];
        return await response.Content.ReadFromJsonAsync<ApiErrorResult>() ?? ErrorResultHelper.CreateGeneralError();
    }
}
