using FreshShop.Business.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoryFilter([FromQuery]string languageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categories = await _categoryService.GetAllCategoryFilter(languageId);
            return Ok(categories);
        }
    }
}
