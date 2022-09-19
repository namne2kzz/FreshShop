using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.ProductImage
{
    public class ProductImageCreateRequest
    {       
        public IFormFile ThumbnailImage { get; set; }
    }
}
