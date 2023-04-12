using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Catalog.ProductImage;
using FreshShop.ViewModels.Catalog.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FreshShop.AdminApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IPromotionApiClient _promotionApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IPromotionApiClient promotionApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _promotionApiClient = promotionApiClient;
        }

        public async Task<IActionResult> Index(int? categoryId, string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var session = HttpContext.Session.GetString("Token");
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                CategoryId = categoryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = defaultLanguageId
            };
            var data = await _productApiClient.GetAllByLanguageId(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryApiClient.GetAllCategoryFilter(defaultLanguageId);
            ViewBag.Categories = categories.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.CategoryId
            });
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var categories = await _categoryApiClient.GetAllCategoryFilter(defaultLanguageId);
            ViewBag.Categories = categories.ResultObj.Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString(),
            });
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid) return View();

            var result = await _productApiClient.Create(request);
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
        public async Task<IActionResult> Detail(int id)
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _productApiClient.GetById(id, defaultLanguageId);
            if (result.IsSuccessed)
            {
                var promotion = await _promotionApiClient.GetByProduct(id);
                if (promotion.IsSuccessed)
                {
                    var promotionUpdateRequest = new PromotionUpdateRequest()
                    {
                        Id = promotion.ResultObj.Id,
                        ProductId = promotion.ResultObj.ProductId,
                        FromDate = promotion.ResultObj.FromDate,
                        ExpiredDate = promotion.ResultObj.ExpiredDate,
                        Discount = promotion.ResultObj.Discount,
                        Status = promotion.ResultObj.Status
                    };
                    ViewBag.Promotion = promotionUpdateRequest;
                }
                else
                {
                    ViewBag.Promotion = null;
                }
                

                var images = await _productApiClient.GetListImage(id);
                if (images.IsSuccessed)
                {
                    ViewBag.ListImage = images.ResultObj;
                    return View(result.ResultObj);
                }
            }
            return RedirectToAction("Error", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _productApiClient.GetById(id, defaultLanguageId);
            if (result.IsSuccessed)
            {
                var product = result.ResultObj;
                var updateRequest = new ProductUpdateRequest()
                {
                    Id = id,
                    Description = product.Description,
                    Name = product.Name,
                    SeoAlias = product.SeoAlias,
                    SeoTitle = product.SeoTitle,
                    LanguageId = product.LanguageId
                };
                var categories = await _categoryApiClient.GetAllCategoryFilter(defaultLanguageId);
                ViewBag.Categories = categories.ResultObj.Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString(),
                    Selected = x.CategoryId == product.CategoryID,
                });
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _productApiClient.Update(request);
            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }
            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            if (!ModelState.IsValid) return View();

            var result = await _productApiClient.UpdatePrice(id, newPrice);
            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }
            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {

            var result = await _productApiClient.Delete(id);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage(int productId, IFormFile thumbnailImage)
        {
            if (!ModelState.IsValid) return RedirectToAction("Detail", "Product", new { id = productId });

            var result = await _productApiClient.AddImage(productId, thumbnailImage);
            if (result.IsSuccessed) return RedirectToAction("Detail", "Product", new { id = productId });

            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Detail", "Product", new { id = productId });

        }

        [HttpPost]
        public async Task<JsonResult> DeleteImage(string productImageDeleteRequest)
        {
            var request = JsonConvert.DeserializeObject<ProductImageDeleteRequest>(productImageDeleteRequest);

            var result = await _productApiClient.DeleteImage(request.ProductId, request.ImageId);
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
