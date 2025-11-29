using RCL.Data.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDTO dto);
        Task<bool> RegisterAsync(RegisterDTO dto);
    }
}
