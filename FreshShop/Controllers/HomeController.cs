using FreshShop.ApiIntergration;
using FreshShop.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogApiClient _blogApiClient;       
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger, IBlogApiClient blogApiClient, IProductApiClient productApiClient)
        {
            _logger = logger;
            _blogApiClient = blogApiClient;          
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var blogs = await _blogApiClient.GetAllLatest();
            if (blogs.IsSuccessed)
            {
                ViewBag.LatestBlogs = blogs.ResultObj;
            }

            var sale = await _productApiClient.GetAllSale(culture);
            if (sale.IsSuccessed)
            {
                ViewBag.Sale = sale.ResultObj.Take(4).ToList();
            }

            var latest = await _productApiClient.GetAllLatest(culture);
            if (latest.IsSuccessed)
            {
                ViewBag.Latest = latest.ResultObj.Take(4).ToList();
            }

            var bestSeller = await _productApiClient.GetAllBestSeller(culture);
            if (bestSeller != null)
            {
                ViewBag.BestSeller = bestSeller.ResultObj.Take(4).ToList();
            }
            else
            {
                ViewBag.BestSeller = null;
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
