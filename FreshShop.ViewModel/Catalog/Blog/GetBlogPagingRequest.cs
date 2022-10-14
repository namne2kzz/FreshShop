using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Blog
{
    public class GetBlogPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
