using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class OrderDetailCreateRequest
    {     
        public int CartID { get; set; }

        public int ProductID { get; set; }

        public int? PromotionID { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}
