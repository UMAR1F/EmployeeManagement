using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ClientLibrary.Helper;
using ClientLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Implementations
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string AuthUrl = "api/Authentication";
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync ($"{AuthUrl}/register", user);
            if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error Occcuer");
            return await result.Content.ReadFromJsonAsync<GeneralResponse>()!;

        }
        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var httpClient=getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", user);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error Occcuer");
            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }
        public async Task<LoginResponse> RefreshToken(RefreshToken token)
        {

            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/refresh-token",token);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error Occcuer");
            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }
        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/weatherforecast");
            return result!;
        }
    }
}
