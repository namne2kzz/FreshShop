using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class CheckoutModel
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
