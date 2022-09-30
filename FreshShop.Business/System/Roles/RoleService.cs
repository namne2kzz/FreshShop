using FreshShop.Data.Entities;
using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManage;
      

        public RoleService(RoleManager<AppRole> roleManage)
        {
            _roleManage = roleManage;          
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _roleManage.Roles.Select(x => new RoleViewModel()
            {
                Id=x.Id,
                Name=x.Name,
                Description=x.Description
            }).ToListAsync();

            return roles;
        }

      
    }
}
