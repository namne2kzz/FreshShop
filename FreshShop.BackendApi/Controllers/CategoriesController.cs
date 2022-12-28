using FreshShop.Business.Catalog.Categories;
using FreshShop.ViewModels.Catalog.Category;
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

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllCategoryByLanguageId([FromQuery] GetCategoryPagingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categories = await _categoryService.GetAllPagingByLanguageId(request);
            return Ok(categories);
        }

        [HttpGet("client/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string languageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categories = await _categoryService.GetAll(languageId);
            return Ok(categories);
        }

        [HttpGet("client/{languageId}/tree")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTree([FromQuery] string languageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tree = await _categoryService.GetAllTree(languageId);
            return Ok(tree);
        }



        [HttpGet("{languageId}/{categoryId}")]
        public async Task<IActionResult> GetById([FromQuery]string languageId, int categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = await _categoryService.GetById(categoryId, languageId);
            return Ok(category);

        }

        [HttpGet("{languageId}/{categoryId}/child")]
        public async Task<IActionResult> GetAllChildByCategoryId([FromQuery] GetAllChildRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var children = await _categoryService.GetAllChildByCategoryId(request);
            return Ok(children);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryId = await _categoryService.Create(request);
            if (categoryId == 0)
            {
                return BadRequest(categoryId);
            }
            var category = await _categoryService.GetById(categoryId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = categoryId }, category.ResultObj);

        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _categoryService.Delete(categoryId);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(int categoryId, [FromBody] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("{categoryId}")]
        public async Task<IActionResult> UpdateStatus(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.UpdateStatus(categoryId);
            return Ok(result);
        }
    }
}
