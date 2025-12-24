using RCL.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyColl.MAUI
{
    public class MauiStorageService : IMyStorageService
    {
        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            await SecureStorage.Default.SetAsync(key, json);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var value = await SecureStorage.Default.GetAsync(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

        public async Task RemoveItemAsync(string key) => SecureStorage.Default.Remove(key);
    }
}
