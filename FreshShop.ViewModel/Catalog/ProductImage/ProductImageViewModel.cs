using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.ProductImage
{
    public class ProductImageViewModel
    {
        public int ProductId { get; set; }

        public int ProductImageId { get; set; }

        public string ImagePath { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
