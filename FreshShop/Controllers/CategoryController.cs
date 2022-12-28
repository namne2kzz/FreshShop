using FreshShop.ApiIntergration;
using FreshShop.ViewModels.Catalog.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryApiClient _categoryApClient;
        private string culture = CultureInfo.CurrentCulture.Name;

        public CategoryController(ICategoryApiClient categoryApiClient, ILogger<CategoryController> logger)
        {
            _logger = logger;
            _categoryApClient = categoryApiClient;
        }
        public async Task<IActionResult> Index()
        {
            
            var categories = await _categoryApClient.GetAll(culture);
            if (categories.IsSuccessed)
            {
                return View(categories.ResultObj.ToList());
            }
            return RedirectToAction("Index", "Home");
        }       
    }
}
