using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public List<int> CategoryId { get; set; }
    }
}
