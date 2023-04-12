using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class UserCheckoutViewModel
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ShipEmail { get; set; }

        public string ShipPhone { get; set; }

        public string ShipAddress { get; set; }

        public int? CouponId { get; set; }

        public decimal? CouponDiscount { get; set; }

        public decimal? Discount { get; set; }

        public string ShippingType { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal Total { get; set; }       
    }
}
