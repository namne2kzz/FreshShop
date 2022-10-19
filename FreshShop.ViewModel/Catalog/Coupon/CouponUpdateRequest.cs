using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Coupon
{
    public class CouponUpdateRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }      

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ExpiredDate { get; set; }
      
    }
}
