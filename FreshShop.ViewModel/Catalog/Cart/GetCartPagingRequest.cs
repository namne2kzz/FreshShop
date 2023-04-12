using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class GetCartPagingRequest : PagingRequestBase
    {
        public Guid UserId { get; set; }

        public string LanguageId { get; set; }

        public string Keyword { get; set; }
    }
}
