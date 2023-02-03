using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class CartUpdateRequest
    {
        public int Id { get; set; }

        public int Quantity { get; set; }
    }
}
