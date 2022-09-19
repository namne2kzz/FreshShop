using FreshShop.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Users
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequest request);

        public Task<bool> Register(RegisterRequest request);
    }
}
