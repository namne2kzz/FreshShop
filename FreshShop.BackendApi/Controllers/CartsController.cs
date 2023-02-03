using FreshShop.Business.Catalog.Cart;
using FreshShop.ViewModels.Catalog.Cart;
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
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllByUser([FromQuery] GetCartPagingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var carts = await _cartService.ListCartByUser(request);
            return Ok(carts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _cartService.Create(request);
            if (!result.IsSuccessed) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCart([FromBody] CartUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _cartService.UpdateCart(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveItem([FromQuery]int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _cartService.RemoveItem(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteCart([FromQuery] Guid userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _cartService.DeleteCart(userId);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("cart/{id}")]
        public async Task<IActionResult> GetCartById([FromQuery]int id, string languageId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cart = await _cartService.GetCartById(id, languageId);
            if (cart == null) return BadRequest(cart);
            return Ok(cart);
        }

        [HttpPost("order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var orderId = await _cartService.CreateOrder(request);
            if (orderId > 0) return Ok(orderId);          
            return BadRequest(orderId); 
        }

        [HttpPost("orderdetail/{orderid}")]
        public async Task<IActionResult> CreateOrderDetail([FromQuery] int orderid,[FromBody] List<OrderDetailCreateRequest> requests)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var orderdetail = await _cartService.CreateOrderDetail(orderid, requests);
            if (orderdetail.IsSuccessed) return Ok(orderdetail);
            return BadRequest(orderdetail);

        }

    }
}
