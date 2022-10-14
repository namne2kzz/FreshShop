using FreshShop.AdminApp.Services;
using FreshShop.ViewModels.Catalog.Category;
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
    public class CategoryController : BaseController
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var session = HttpContext.Session.GetString("Token");
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var request = new GetCategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = defaultLanguageId
            };
            var data = await _categoryApiClient.GetAllCategoryByLanguageId(request);
            ViewBag.Keyword = keyword;

            return View(data.ResultObj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            if (request.ThumbnailImage == null)
            {
                ModelState.AddModelError("", "Hình ảnh không được rỗng");
                return View(request);
            }

            var result = await _categoryApiClient.Create(request);
            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");

            }
            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var result = await _categoryApiClient.GetById(defaultLanguageId, id);

            var request = new GetAllChildRequest()
            {                            
                LanguageId = defaultLanguageId,
                CategoryId=id
            };
            var subCategory = await _categoryApiClient.GetAllChildByCategoryId(request);
            if (subCategory.IsSuccessed)
            {
                ViewBag.SubCategory = subCategory.ResultObj;
            }
            else
            {
                return RedirectToAction("Index");
            }
            if (result.IsSuccessed) return View(result.ResultObj);

            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {

            var result = await _categoryApiClient.Delete(id);
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

        public async Task<IActionResult> Update(int id)
        {
            var defaultLanguageId = HttpContext.Session.GetString("DefaultLanguageId");
            var category = await _categoryApiClient.GetById(defaultLanguageId, id);
            if (!category.IsSuccessed)
            {
                SetAlert(category.Message, "warning");
                return RedirectToAction("Index");
            }
            var categoryUpdateRequest = new CategoryUpdateRequest()
            {
                CategoryId = category.ResultObj.CategoryId,
                CategoryName = category.ResultObj.CategoryName,
                ParentId = category.ResultObj.ParentId,
                LanguageId = category.ResultObj.LanguageId,
                SeoAlias = category.ResultObj.SeoAlias,
                SeoTitle = category.ResultObj.SeoTitle,
            };
            return View(categoryUpdateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _categoryApiClient.Update(request);

            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }

            SetAlert(result.Message, "warning");           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateShowOnHome(int categoryId)
        {
            var result = await _categoryApiClient.UpdateStatus(categoryId);          
            return Json(new
            {
                status = result
            });
        }
    }
}
