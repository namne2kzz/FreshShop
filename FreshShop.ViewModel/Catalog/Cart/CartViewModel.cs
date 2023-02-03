using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class CartViewModel
    {
        public int ID { get; set; }

        public Guid UserId { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }
      
        public DateTime CreatedDate { get; set; }
    }
}
