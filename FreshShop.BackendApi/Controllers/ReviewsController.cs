using FreshShop.Business.Catalog.Reviews;
using FreshShop.ViewModels.Catalog.Review;
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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllReviewByProduct([FromQuery]int productId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reviews = await _reviewService.GetAllByProduct(productId);
            return Ok(reviews);
        }

        [HttpGet("review/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var review = await _reviewService.GetById(id);
            if (review.IsSuccessed) return Ok(review);
            return BadRequest(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewId = await _reviewService.Create(request);
            if (reviewId < 0) return BadRequest(reviewId);
            var review = await _reviewService.GetById(reviewId);
            return CreatedAtAction(nameof(GetReviewById), new { id = reviewId }, review.ResultObj.ID);
        }


    }
}
