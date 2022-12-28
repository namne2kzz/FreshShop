using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Catalog.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FreshShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IReviewApiClient _reviewApiClient;
        private string _culture = CultureInfo.CurrentCulture.Name;

        public ProductController(ILogger<ProductController> logger, IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IReviewApiClient reviewApiClient)
        {
            _logger = logger;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _reviewApiClient = reviewApiClient;
        }

        public async Task<IActionResult> Index(int? sortId, int? categoryId, string keyword, int pageIndex = 1, int pageSize = 3)
        {
            var request = new GetPublicProductPagingRequest()
            {
                SortId = sortId,
                CategoryId = categoryId,
                Keyword = keyword,
                LanguageId = _culture,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.Keyword = keyword;
            var products = await _productApiClient.GetAll(request);
            if (products.IsSuccessed)
            {
                var tree = await _categoryApiClient.GetAllTree(_culture);
                if (tree.IsSuccessed)
                {
                    ViewBag.Tree = tree.ResultObj;
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.TotalRecord = products.ResultObj.TotalRecord;
                return View(products.ResultObj);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Detail(int productId)
        {
            var product = await _productApiClient.GetByIdClient(productId, _culture);
            var images = await _productApiClient.GetListImageClient(productId);
            var reviews = await _reviewApiClient.GetAllReviewByProduct(productId);
            var request = new GetRelatedProductRequest()
            {
                ProductId = productId,
                LanguageId = _culture
            };         

            var related = await _productApiClient.GetAllRelated(request);
            if (product.IsSuccessed && images.IsSuccessed)
            {
                ViewBag.Image = images.ResultObj;
                ViewBag.Related = related.ResultObj;
                if (reviews == null)
                {
                    ViewBag.Reviews = null;
                }
                else
                {
                    ViewBag.Reviews = reviews.ResultObj;
                }
               

                return View(product.ResultObj);
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> AddReview(int productId, string message)
        {
            var request = new ReviewCreateRequest()
            {
                Message = message,
                ProductID = productId,
                UserId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };
            var result = await _reviewApiClient.Create(request);          

            return RedirectToAction("Detail", "Product",new { productId=productId});
        }
    }
}
