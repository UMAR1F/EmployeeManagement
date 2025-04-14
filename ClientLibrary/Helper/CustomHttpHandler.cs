using BaseLibrary.DTOs;
using ClientLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
   public class CustomHttpHandler
        (GetHttpClient getHttpClient, LocalStorageService localStorageService, IUserAccountService accountService):DelegatingHandler
    {
        protected async override Task <HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri!.Contains("login");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("refresh-token");
            if(loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken);
            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode== HttpStatusCode.Unauthorized)
            {
                //Get Token from Local  Storage
                var stringToken = await localStorageService.GetToken();
                if (stringToken != null) return result;

                //checking if the header contain token
                string token = string.Empty;
                try
                {
                    token = request.Headers.Authorization!.Parameter!;
                }
                catch
                {

                }
                var deserializedToken=Serialization.DeserializeJsonString<UserSession>(stringToken!);
                if (deserializedToken != null) return result;

                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" , deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                //Call RefreshToken
                var newjwtToken = await GetReshToken(deserializedToken!.RefreshToken!);
                if (string.IsNullOrEmpty(newjwtToken)) return result;

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                return await base.SendAsync(request, cancellationToken);
            }
            return result;
        }

        private async Task <string> GetReshToken(string refreshToken)
        {
            var result = await accountService.RefreshToken(new RefreshToken() { Token = refreshToken });
            string serializedToken = Serialization.SerializeObj(new UserSession()
            { Token = result.Token, RefreshToken = result.RefreshToken });
            await localStorageService.SetToken(serializedToken);
            return result.Token;
        }
    }
}
