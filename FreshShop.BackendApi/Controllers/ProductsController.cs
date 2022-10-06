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
        private readonly IProductService _productService;


        public ProductsController(IProductService productService)
        {
            _productService = productService;

        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllByLanguage([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllByLanguageId(request);
            return Ok(products);
        }


        [HttpGet("{languageId}/{categoryId}")]
        public async Task<IActionResult> GetAllByCategory([FromQuery] GetPublicProductPagingRequest request, string languageId)
        {
            var products = await _productService.GetAllByCategoryId(request, languageId);
            return Ok(products);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetById([FromQuery] int productId, string languageId)
        {
            var product = await _productService.GetById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Cannot find product");
            }
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var productId = await _productService.Create(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            var product = await _productService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product.ResultObj);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(int productId, [FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _productService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _productService.Delete(productId);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {

            var result = await _productService.UpdatePrice(productId, newPrice);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch("stock/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateStock(int productId, int quantity)
        {

            var affectedRow = await _productService.UpdateStock(productId, quantity);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("sold/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateSold(int productId, int quantity)
        {

            var affectedRow = await _productService.UpdateSold(productId, quantity);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateViewCount(int productId)
        {

            var affectedRow = await _productService.UpdateViewCount(productId);
            if (!affectedRow)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{productId}/images/{imageId})")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image == null)
            {
                return BadRequest("Cannot fid image");
            }
            return Ok(image);
        }

        [HttpPost("{productId}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImage([FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var imageId = await _productService.AddImage(request);
            if (imageId == 0)
            {
                return BadRequest();
            }
            var image = await _productService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var result = await _productService.DeleteImage(imageId);

            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);


        }

        [HttpPatch("{productId}/images/{imageId}")]
        public async Task<IActionResult> ChangeImageStatus(int imageId)
        {
            var affectedRow = await _productService.ChangeImageStatus(imageId);
            if (affectedRow == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{productId}/images")]
        public async Task<IActionResult> GetListImage([FromQuery] int productId)
        {
            var images = await _productService.GetListImage(productId);
            if (images.Count > 0) return Ok(images);
            return BadRequest(images);
        }



    }
}

