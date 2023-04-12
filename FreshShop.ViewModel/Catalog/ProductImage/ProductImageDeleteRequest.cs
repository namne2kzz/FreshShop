using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.ProductImage
{
    public class ProductImageDeleteRequest
    {
        public int ProductId { get; set; }

        public int ImageId { get; set; }
    }
}
