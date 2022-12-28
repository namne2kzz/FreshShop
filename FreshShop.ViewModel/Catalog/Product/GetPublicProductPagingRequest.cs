using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {       
        public int? SortId { get; set; }

        public int? CategoryId { get; set; }

        public string Keyword { get; set; }

        public string LanguageId { get; set; }
    }
}
