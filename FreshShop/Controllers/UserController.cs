using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Address;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FreshShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IAddressApiClient _addressApiClient;
        public UserController(IAddressApiClient addressApiClient)
        {
            _addressApiClient = addressApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Address()
        {
            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var addresses = await _addressApiClient.GetAllByUserId(userId);
            foreach(var item in addresses.ResultObj)
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
            var listProvince = await _addressApiClient.GetListProvince();
            ViewBag.ListProvince = listProvince.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code.ToString(),
            });
            if (addresses.IsSuccessed) return View(addresses.ResultObj);
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> LoadDistrict(int id)
        {
            var listDistrict = await _addressApiClient.GetListDistrictByProvince(id);
            var data = listDistrict.ResultObj;

            return Json(new
            {
                status = true,
                data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(string province, string district, string detail)
        {
            if (string.IsNullOrEmpty(district))
            {
                TempData["CreateAddressError"] = true;
                return RedirectToAction("Address");
            }
            var request = new AddressCreateRequest()
            {
                Detail = detail,
                Districtd = int.Parse(district),
                ProvinceId = int.Parse(province),
                UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value),
            };
            var result = await _addressApiClient.Create(request);
            if (result.IsSuccessed) return RedirectToAction("Address");
            TempData["CreateAddressError"] = true;
            return RedirectToAction("Address"); 
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


    }
}
