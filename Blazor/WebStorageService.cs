using RCL.Data.Interfaces;

namespace BlazorWEB
{
    public class WebStorageService : IMyStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _blazored;
        public WebStorageService(Blazored.LocalStorage.ILocalStorageService blazored) => _blazored = blazored;

        public async Task SetItemAsync<T>(string key, T value) => await _blazored.SetItemAsync(key, value);
        public async Task<T> GetItemAsync<T>(string key) => await _blazored.GetItemAsync<T>(key);
        public async Task RemoveItemAsync(string key) => await _blazored.RemoveItemAsync(key);
    }
}
