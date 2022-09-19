using FreshShop.Business.Catalog.Products;
using FreshShop.Data.Entities;
using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Catalog.ProductImage;
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
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;

        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }     

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPublicProductPagingRequest request, string languageId)
        {
            var products = await _publicProductService.GetAllByCategoryId(request, languageId);
            return Ok(products);
        }

        [HttpGet("{productId}/{languageId})")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Cannot fid product");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var productId = await _manageProductService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            var product = await _manageProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById),new { id=productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var affectedRow = await _manageProductService.Update(request);
            if (affectedRow == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedRow = await _manageProductService.Delete(productId);
            if (affectedRow == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            
            var affectedRow = await _manageProductService.UpdatePrice(productId,newPrice);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("stock/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateStock(int productId, int quantity)
        {

            var affectedRow = await _manageProductService.UpdateStock(productId, quantity);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("sold/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateSold(int productId, int quantity)
        {

            var affectedRow = await _manageProductService.UpdateSold(productId, quantity);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateViewCount(int productId)
        {

            var affectedRow = await _manageProductService.UpdateViewCount(productId);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{productId}/images/{imageId})")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            var image = await _manageProductService.GetImageById(imageId);
            if (image == null)
            {
                return BadRequest("Cannot fid image");
            }
            return Ok(image);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddImage([FromForm]ProductImageCreateRequest request, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var imageId = await _manageProductService.AddImage(productId, request);
            if (imageId == 0)
            {
                return BadRequest();
            }
            var image = await _manageProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var affectedRow = await _manageProductService.DeleteImage(imageId);
            if (affectedRow == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("{productId}/images/{imageId}")]
        public async Task<IActionResult> ChangeImageStatus(int imageId)
        {
            var affectedRow = await _manageProductService.ChangeImageStatus(imageId);
            if (affectedRow == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetListImage(int productId)
        {
            var images = await _manageProductService.GetListImage(productId);
            return Ok(images);
        }
    }
}
