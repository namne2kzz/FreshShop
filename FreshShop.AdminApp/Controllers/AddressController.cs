using FreshShop.AdminApp.Services;
using FreshShop.ViewModels.Catalog.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Controllers
{
    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IAddressApiClient _addressApiClient;

        public AddressController(IAddressApiClient addressApiClient)
        {
            _addressApiClient = addressApiClient;
        }

        public async Task<IActionResult> Create()
        {
            var listProvince = await _addressApiClient.GetProvince();
            ViewBag.ListProvince = listProvince.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code.ToString(),
            });
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddressCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _addressApiClient.Create(request);

            if (result.IsSuccessed) return RedirectToAction("Detail", "User", new { id = request.UserId });

            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var address = await _addressApiClient.GetById(id);
            if (!address.IsSuccessed) return RedirectToAction("Index", "User");

            var listProvince = await _addressApiClient.GetProvince();
            ViewBag.ListProvince = listProvince.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code.ToString(),
                Selected=address.IsSuccessed && address.ResultObj.ProvinceId==x.Code
            });

            var listDistrict = await _addressApiClient.GetDistrict(address.ResultObj.ProvinceId);
            ViewBag.ListDistrict = listDistrict.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code.ToString(),
                Selected = address.IsSuccessed && address.ResultObj.DistrictId == x.Code
            });

            var addressUpdateRequest = new AddressUpdateRequest()
            {
                Id = id,
                ProvinceId = address.ResultObj.ProvinceId,
                Districtd = address.ResultObj.DistrictId,
                Detail = address.ResultObj.Detail
            };

            return View(addressUpdateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AddressUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var address = await _addressApiClient.GetById(request.Id);

            var result = await _addressApiClient.Update(request);
            if (result.IsSuccessed) return RedirectToAction("Detail", "User", new { id=address.ResultObj.UserId});

            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _addressApiClient.Delete(id);
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status = true,
                });
            }
            return Json(new
            {
                status = false,
            });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeDefaultAddress(int id)
        {
            var result = await _addressApiClient.ChangeDefaultAddress(id);
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status = true,
                });
            }
            return Json(new
            {
                status = false,
            });
        }
           

        [HttpPost]
        public async Task<JsonResult> LoadDistrict(int id)
        {
            var listDistrict = await _addressApiClient.GetDistrict(id);
            var data = listDistrict.ResultObj;
            
            return Json(new
            {
                status=true,
                data=data
            });
        }


       
    }
}
