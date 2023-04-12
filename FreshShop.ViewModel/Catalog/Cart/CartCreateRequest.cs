using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class CartCreateRequest
    {
        public Guid UserId { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public string LanguageId { get; set; }



    }
}
