using FreshShop.Data.EF;
using FreshShop.Data.Entities;
using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using FreshShop.ViewModels.System.Users;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly FreshShopDbContext _context;


        public RoleService(RoleManager<AppRole> roleManage, UserManager<AppUser> userManager, FreshShopDbContext context)
        {
            _roleManage = roleManage;
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(RoleCreateRequest request)
        {
            var roles = await _roleManage.Roles.Select(x => x.Name).ToListAsync();

            var roleExist = await _roleManage.RoleExistsAsync(request.Name);
            if (!roleExist)
            {
                var role = new AppRole()
                {
                    Id = new Guid(),
                    Name = request.Name,
                    ConcurrencyStamp = new Guid().ToString(),
                    Description = request.Description,
                    NormalizedName = request.Name,
                };
                await _roleManage.CreateAsync(role);
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Thêm mới không thành công");

        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var role = await _roleManage.FindByIdAsync(id.ToString());
            if (role == null) return new ApiErrorResult<bool>("Không tìm thấy nhóm quyền để xóa");

            var result = await _roleManage.DeleteAsync(role);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa thất bại");
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _roleManage.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                Description = x.Description,
                ConcurrencyStamp = x.ConcurrencyStamp
            }).ToListAsync();

            return roles;
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetAllPagingByRole(GetUserPagingByRoleRequest request)
        {
            var role = await _roleManage.FindByIdAsync(request.Id.ToString());
            if (role == null) return new ApiErrorResult<PagedResult<UserViewModel>>("Không tìm thấy nhóm quyền");
            var users = await _userManager.GetUsersInRoleAsync(role.Name);

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                users = users.Where(x => x.UserName.Contains(request.Keyword) || x.Firstname.Contains(request.Keyword) || x.Lastname.Contains(request.Keyword)).ToList();
            }

            var data = users.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    FirstName = x.Firstname,
                    LastName = x.Lastname,
                    Dob = x.Dob,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    ImagePath = x.ImagePath,
                    UserName = x.UserName,
                }).ToList();

            int totalRow = users.Count();
            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
        }
    }
}
