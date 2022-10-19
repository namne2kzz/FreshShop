using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Contact
{
    public class GetContactPagingRequest :PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
