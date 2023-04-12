using FreshShop.Business.Common;
using FreshShop.Data.Entities;
using FreshShop.Utilities;
using FreshShop.Utilities.Exceptions;
using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using FreshShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;


        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config, IStorageService storageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _storageService = storageService;
            _config = config;

        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var adminRole = await _roleManager.FindByNameAsync(SystemConstants.AdminRoleName);
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<string>("Đăng nhập không đúng");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded) return new ApiErrorResult<string>("Đăng nhập không đúng");

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (await _userManager.IsInRoleAsync(user, adminRole.Name))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.GivenName,user.Firstname),
                        new Claim(ClaimTypes.Role,string.Join(";",roles)),
                        new Claim(ClaimTypes.Name,request.UserName),
                        new Claim(ClaimTypes.Uri,user.ImagePath),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                        _config["Tokens:Issuer"],
                        claims,
                        expires: DateTime.Now.AddHours(3),
                        signingCredentials: creds);

                    return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            return new ApiErrorResult<string>("Tài khoản không có quyền đăng nhập");
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Thất bại");


        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserViewModel>("Tài khoản không tồn tại");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var result = new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Dob = user.Dob,
                UserName = user.UserName,
                ImagePath = user.ImagePath,
                Roles = roles
            };
            return new ApiSuccessResult<UserViewModel>(result);
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.Firstname.Contains(request.Keyword) || x.Lastname.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    FirstName = x.Firstname,
                    LastName = x.Lastname,
                    UserName = x.UserName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    ImagePath = x.ImagePath,

                }).ToListAsync();

            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {

            if (await _userManager.FindByNameAsync(request.UserName) != null) return new ApiErrorResult<bool>("Tài khoản đã tồn tại");

            if (await _userManager.FindByEmailAsync(request.Email) != null) return new ApiErrorResult<bool>("Email đã tồn tại");

            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Lastname = request.LastName,
                Firstname = request.FirstName,
                UserName = request.UserName,

            };
            if (request.ThumbnailImage != null)
            {
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded) return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Đăng ký không thành công");

        }



        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id)) return new ApiErrorResult<bool>("Email đã tồn tại");


            var user = await _userManager.FindByIdAsync(id.ToString());

            user.Dob = request.Dob;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Lastname = request.LastName;
            user.Firstname = request.FirstName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return new ApiErrorResult<bool>("Tài khoản không tồn tại");

            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = request.Roles.Where(x => x.Selected == true).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<string>> AuthenticateClient(LoginRequest request)
        {
            
            
            var roleUser = await _roleManager.FindByNameAsync(SystemConstants.UserRoleName);
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<string>("Đăng nhập không đúng");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded) return new ApiErrorResult<string>("Đăng nhập không đúng");

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var utem in roles)
            {
                if (await _userManager.IsInRoleAsync(user, roleUser.Name) || roles == null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.GivenName,user.Firstname),
                        new Claim(ClaimTypes.Role,string.Join(";",roles)),
                        new Claim(ClaimTypes.Name,request.UserName),
                        new Claim(ClaimTypes.Uri,user.ImagePath),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                        _config["Tokens:Issuer"],
                        claims,
                        expires: DateTime.Now.AddHours(3),
                        signingCredentials: creds);

                    return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            return new ApiErrorResult<string>("Tài khoản không có quyền đăng nhập");

        }

        public async Task<ApiResult<bool>> RegisterClient(RegisterRequest request)
        {
            var roleUser = await _roleManager.FindByNameAsync(SystemConstants.UserRoleName);
            if (await _userManager.FindByNameAsync(request.UserName) != null) return new ApiErrorResult<bool>("Tài khoản đã tồn tại");

            if (await _userManager.FindByEmailAsync(request.Email) != null) return new ApiErrorResult<bool>("Email đã tồn tại");

            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Lastname = request.LastName,
                Firstname = request.FirstName,
                UserName = request.UserName,

            };
            if (request.ThumbnailImage != null)
            {
                user.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
               await _userManager.AddToRoleAsync(user,roleUser.Name);
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Đăng ký không thành công. "+result.Errors);
        }
    }
}
