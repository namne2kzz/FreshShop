using FreshShop.Business.Catalog.Blogs;
using FreshShop.ViewModels.Catalog.Blog;
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
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetBlogPagingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var blogs = await _blogService.GetAllPaging(request);
            if (blogs.IsSuccessed) return Ok(blogs);
            return BadRequest(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var blog = await _blogService.GetById(id);
            if (blog.IsSuccessed) return Ok(blog);
            return BadRequest(blog);

        }      

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] BlogCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var blogId = await _blogService.Create(request);
            if (blogId == 0)
            {
                return BadRequest(blogId);
            }
            var blog = await _blogService.GetById(blogId);

            return CreatedAtAction(nameof(GetById), new { id = blogId }, blog.ResultObj);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _blogService.Delete(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _blogService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _blogService.ChangeStatus(id);
            return Ok(result);
        }
    }
}
