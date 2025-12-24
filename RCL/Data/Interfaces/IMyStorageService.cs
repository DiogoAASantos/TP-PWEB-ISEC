using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IMyStorageService 
    {
        Task SetItemAsync<T>(string key, T value);
        Task<T> GetItemAsync<T>(string key);
        Task RemoveItemAsync(string key);
    }
}
