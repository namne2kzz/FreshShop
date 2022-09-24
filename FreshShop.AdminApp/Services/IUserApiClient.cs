using FreshShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
    }
}
