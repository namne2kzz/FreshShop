using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Category
{
    public class CategoryUpdateRequest
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int? ParentId { get; set; }

        //public bool IsShowOnHome { get; set; }

        //public IFormFile ThumbnailImage { get; set; }

        public string SeoTitle { get; set; }

        public string SeoAlias { get; set; }

        public string LanguageId { get; set; }
    }
}
