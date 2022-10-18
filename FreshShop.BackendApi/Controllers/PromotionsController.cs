using FreshShop.Business.Catalog.Promotions;
using FreshShop.ViewModels.Catalog.Promotion;
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
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var promotion = await _promotionService.GetById(id);
            if (promotion.IsSuccessed) return Ok(promotion);
            return BadRequest(promotion);

        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetByProductId([FromQuery] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var promotion = await _promotionService.GetByProductId(id);
            if (promotion.IsSuccessed) return Ok(promotion);
            return BadRequest(promotion);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PromotionCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var promotionId = await _promotionService.Create(request);

            if (promotionId < 0) return BadRequest(promotionId);
            
            var promotion = await _promotionService.GetById(promotionId);

            return CreatedAtAction(nameof(GetById), new { id = promotionId }, promotion.ResultObj);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _promotionService.Delete(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PromotionUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _promotionService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }
    }
}
