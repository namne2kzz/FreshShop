using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Blog;
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
    public class BlogController : BaseController
    {
        private readonly IBlogApiClient _blogApiClient;

        public BlogController(IBlogApiClient blogApiClient)
        {
            _blogApiClient = blogApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var session = HttpContext.Session.GetString("Token");          
            var request = new GetBlogPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize               
            };
            var data = await _blogApiClient.GetAll(request);
            ViewBag.Keyword = keyword;

            return View(data.ResultObj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(BlogCreateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            if (request.ThumbnailImage == null)
            {
                ModelState.AddModelError("", "Hình ảnh không được rỗng");
                return View(request);
            }

            var result = await _blogApiClient.Create(request);
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
            var result = await _blogApiClient.GetById(id);           
                     
            if (result.IsSuccessed) return View(result.ResultObj);

            SetAlert(result.Message, "warning");
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {

            var result = await _blogApiClient.Delete(id);
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
            var blog = await _blogApiClient.GetById(id);
            if (!blog.IsSuccessed)
            {
                SetAlert(blog.Message, "warning");
                return RedirectToAction("Index");
            }
            var blogUpdateRequest = new BlogUpdateRequest()
            {
                Id = blog.ResultObj.Id,
                Title = blog.ResultObj.Title,
                Content = blog.ResultObj.Content              
            };
            return View(blogUpdateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BlogUpdateRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var result = await _blogApiClient.Update(request);

            if (result.IsSuccessed)
            {
                SetAlert(result.Message, "success");
                return RedirectToAction("Index");
            }

            SetAlert(result.Message, "warning");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(int id)
        {
            var result = await _blogApiClient.UpdateStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
