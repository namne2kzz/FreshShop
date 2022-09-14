using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class ProductViewModel
    {
        public int ID { get; set; }

        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Name { get; set; }

        public int Stock { get; set; }

        public int Sold { get; set; }

        public string Unit { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Description { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }

    }
}
