using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();

      
    }
}
