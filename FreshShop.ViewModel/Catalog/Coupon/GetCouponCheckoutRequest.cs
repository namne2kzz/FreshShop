using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Coupon
{
    public class GetCouponCheckoutRequest
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public decimal Discount { get; set; }
    }
}
