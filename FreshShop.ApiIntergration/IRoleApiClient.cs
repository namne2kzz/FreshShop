using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using FreshShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();

        Task<ApiResult<PagedResult<UserViewModel>>> GetAllPagingByRole(GetUserPagingByRoleRequest request);

        Task<ApiResult<bool>> Create(RoleCreateRequest request);

        Task<ApiResult<bool>> Delete(Guid id);
    }
}
