using FreshShop.AdminApp.Services;
using FreshShop.ViewModels.Catalog.Coupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Controllers
{
    [Authorize]
    public class CouponController : BaseController
    {
        private readonly ICouponApiClient _couponApiClient;

        public CouponController(ICouponApiClient couponApiClient)
        {
            _couponApiClient = couponApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex=1, int pageSize=1)
        {
            var resquest = new GetCouponPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var result = await _couponApiClient.GetAllPaging(resquest);
            ViewBag.Keyword = keyword;
            return View(result.ResultObj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            //var coupon = await _couponApiClient.GetCouponByCode(request.Code);
            //if (coupon.IsSuccessed)
            //{
            //    ModelState.AddModelError("", "Mã giảm giá không được trùng");
            //    return View(request);
            //}

            var result = await _couponApiClient.Create(request);
            if (result > 0)
            {
                SetAlert("Thêm mới thành công", "success");
                return RedirectToAction("Index");
            }

            SetAlert("Thêm mới không thành công", "warning");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var coupon = await _couponApiClient.GetById(id);
            var response = new CouponUpdateRequest()
            {
                Id = id,                
                Title = coupon.ResultObj.Title,
                Discount = coupon.ResultObj.Discount,
                Quantity = coupon.ResultObj.Quantity,
                ExpiredDate = coupon.ResultObj.ExpiredDate,
                FromDate = coupon.ResultObj.FromDate,             
            };
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CouponUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _couponApiClient.Update(request);
            if (result.IsSuccessed) 
            {
                SetAlert("Cập nhật thành công", "success");
                return RedirectToAction("Index");
            }

            SetAlert("Cập nhật thành công", "warning");
            return RedirectToAction("Index");
        } 

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _couponApiClient.Delete(id);

            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status = true
                });
            }

            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(int id)
        {
            var result = await _couponApiClient.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
