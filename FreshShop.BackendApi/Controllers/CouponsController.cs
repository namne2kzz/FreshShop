using FreshShop.Business.Catalog.Coupons;
using FreshShop.ViewModels.Catalog.Coupon;
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
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetCouponPagingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var coupons = await _couponService.GetAllPaging(request);
            if (coupons.IsSuccessed) return Ok(coupons);
            return BadRequest(coupons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var coupon = await _couponService.GetById(id);
            if (coupon.IsSuccessed) return Ok(coupon);
            return BadRequest(coupon);

        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode([FromQuery] string code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var coupon = await _couponService.GetCouponByCode(code);
            if (coupon.IsSuccessed) return Ok(coupon);
            return BadRequest(coupon);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CouponCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var couponId = await _couponService.Create(request);
            if (couponId < 0)
            {
                return BadRequest(couponId);
            }
            var coupon = await _couponService.GetById(couponId);

            return CreatedAtAction(nameof(GetById), new { id = couponId }, couponId);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _couponService.Delete(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CouponUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _couponService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQuantity(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _couponService.UpdateQuantity(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("status/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _couponService.ChangeStatus(id);
            return Ok(result);

        }
    }
}
