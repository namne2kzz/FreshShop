using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class ProductCreateRequest
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Description { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }

        public IFormFile ThumbnailImage { get; set; }

    }
}
