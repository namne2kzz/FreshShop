using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Roles;
using FreshShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IAddressApiClient _addressApiClient;

        public UserController(IUserApiClient userApiClient, IRoleApiClient roleApiClient, IAddressApiClient addressApiClient)
        {
            _userApiClient = userApiClient;
            _roleApiClient = roleApiClient;
            _addressApiClient = addressApiClient;
        }
        
        public async Task<IActionResult> Index(string keyword, int pageIndex=1, int pageSize=10)
        {
            var session = HttpContext.Session.GetString("Token");
            var request = new GetUserPagingRequest()
            {              
                Keyword=keyword,
                PageIndex=pageIndex,
                PageSize=pageSize,
            };
            var data = await _userApiClient.GetUserPaging(request);
            ViewBag.Keyword = keyword;
            return View(data.ResultObj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userApiClient.Register(request);
            if (result.IsSuccessed) 
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");

            }
            SetAlert(result.Message, "warning");           
            return RedirectToAction("Index","User");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Id = id,
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };               
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
           
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _userApiClient.Update(request.Id,request);
            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }
            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
           var listAddress = await _addressApiClient.GetAllByUserId(id);
            foreach(var item in listAddress.ResultObj)
            {
                var province = await _addressApiClient.GetProvince(item.ProvinceId);
                var district = await _addressApiClient.GetDistrict(item.DistrictId);
                if (!province.IsSuccessed || !district.IsSuccessed)
                {
                    TempData["AddressError"] = true;
                    return RedirectToAction("Index");
                }
                item.ProvinceName = province.ResultObj.Name;
                item.DistrictName = district.ResultObj.Name;
            }
            ViewBag.ListAddress = listAddress.ResultObj;

            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }

        [HttpDelete]
        [EnableCors("MyPolicy")]
        public async Task<JsonResult> Delete(Guid id)
        {           

            var result = await _userApiClient.Delete(id);
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status=true
                });
            }
            return Json(new
            {
                status = false,              
            });

        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);

            return View(roleAssignRequest);

        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _userApiClient.RoleAssign(request.Id, request);


            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }
            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);
            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            var roles = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();

            foreach (var role in roles.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectedItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = user.ResultObj.Roles.Contains(role.Name)
                });
            }

            return roleAssignRequest;
        }
    }
}
