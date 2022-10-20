using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using FreshShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserViewModel>> GetById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}
