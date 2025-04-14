using Blazored.LocalStorage;



namespace ClientLibrary.Helper
{
   
        public class LocalStorageService(ILocalStorageService localStorageService)
        {
            private const string Storagekey = "authentification-token";
            public async Task <string>GetToken()=> await localStorageService.GetItemAsStringAsync(Storagekey);
            public async Task  SetToken(string item) => await localStorageService.SetItemAsStringAsync(Storagekey, item);
            public async Task RemoveToken()=>await localStorageService.RemoveItemAsync(Storagekey);
        }
    }

