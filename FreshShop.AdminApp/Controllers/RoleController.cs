using FreshShop.AdminApp.Services;
using FreshShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleApiClient _roleApiClient;

        public RoleController(IRoleApiClient roleApiClient)
        {
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var rolse = await _roleApiClient.GetAll();
            return View(rolse.ResultObj);
        }

        public async Task<IActionResult> ListByRole(Guid id, string keyword, int pageIndex=1, int pageSize = 1)
        {
            var request = new GetUserPagingByRoleRequest()
            {
                Id = id,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = await _roleApiClient.GetAllPagingByRole(request);
            if (result.IsSuccessed) return View(result.ResultObj);

            SetAlert(result.Message, "warning");
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _roleApiClient.Create(request);

            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }

            SetAlert(result.Message, "warning");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Guid id)
        {
            var result = await _roleApiClient.Delete(id);

            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = true
            });
        }
    }
}
