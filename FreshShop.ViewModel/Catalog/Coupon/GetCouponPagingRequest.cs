using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Coupon
{
    public class GetCouponPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
