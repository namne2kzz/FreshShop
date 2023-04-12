using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Promotion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Controllers
{
    [Authorize]
    public class PromotionController : BaseController
    {
        private readonly IPromotionApiClient _promotionApiClient;
        private readonly IProductApiClient _productApiClient;
       
        public PromotionController(IPromotionApiClient promotionApiClient, IProductApiClient productApiClient)
        {
            _promotionApiClient = promotionApiClient;
            _productApiClient = productApiClient;
        }

        public IActionResult Create(int id)
        {
            TempData["ProductId"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromotionCreateRequest request)
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            TempData["ProductId"] = request.ProductId;
            if (!ModelState.IsValid) return View(request);

            var product = await _productApiClient.GetById(request.ProductId, defaultLanguageId);

            if (request.Discount > product.ResultObj.Price)
            {
                ModelState.AddModelError("", "Giảm giá không hợp lệ");
                return View(request);
            }

            var result = await _promotionApiClient.Create(request);
            if (result>0)
            {
                SetAlert("Thao tác thành công", "success");
                return RedirectToAction("Index", "Product");
            }

            SetAlert("Sản phẩm này đã tồn tại giảm giá", "warning");
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Update(PromotionUpdateRequest request)
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            if (!ModelState.IsValid)
            {
                SetAlert("Lỗi cập nhật", "warining");
                return RedirectToAction("Index", "Product");
            }

            var product = await _productApiClient.GetById(request.ProductId, defaultLanguageId);

            if (request.Discount > product.ResultObj.Price)
            {
                ModelState.AddModelError("", "Giảm giá không hợp lệ");
                return View(request);
            }

            var result = await _promotionApiClient.Update(request);
            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index", "Product");
            }
            SetAlert(result.Message, "warning");
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _promotionApiClient.Delete(id);
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    status=true
                });
            }
            return Json(new
            {
                status = false
            });
        }
    }
}
