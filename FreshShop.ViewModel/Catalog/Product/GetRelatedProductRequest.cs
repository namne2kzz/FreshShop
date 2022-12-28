using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class GetRelatedProductRequest
    {
        public int ProductId { get; set; }

        public string LanguageId { get; set; }
    }
}
