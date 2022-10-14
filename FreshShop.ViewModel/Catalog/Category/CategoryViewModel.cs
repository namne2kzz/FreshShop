using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Category
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int? ParentId { get; set; }

        public String ImagePath { get; set; }

        public bool IsShownHome { get; set; }

        public string LanguageId { get; set; }

        public string SeoTitle { get; set; }

        public string SeoAlias { get; set; }
    }
}
