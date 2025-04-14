using BaseLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helper
{
    public class GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {
        private const string Headerkey = "Authorization";

        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return client;

            var deserializeToken = Serialization.DeserializeJsonString
                <UserSession>(stringToken);
            if (deserializeToken == null) return client;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializeToken.Token);
            return client;

        }
        public HttpClient GetPublicHttpClient()
        {
            var client = httpClientFactory.CreateClient ("SystemApiClient");
            client.DefaultRequestHeaders.Remove(Headerkey);
            return client;
        }

    }
}
