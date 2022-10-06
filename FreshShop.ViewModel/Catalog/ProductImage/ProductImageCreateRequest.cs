using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.ProductImage
{
    public class ProductImageCreateRequest
    {       
        public int ProductId { get; set; }

        public IFormFile ThumbnailImage { get; set; }
    }
}
