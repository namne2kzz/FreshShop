using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using FreshShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();

        Task<ApiResult<bool>> Create(RoleCreateRequest request);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<PagedResult<UserViewModel>>> GetAllPagingByRole(GetUserPagingByRoleRequest request);
        
    }
}
