using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Cart
{
    public class OrderCreateRequest
    {       
        public Guid UserId { get; set; }
       
        public string ShipName { get; set; }
      
        public string ShipEmail { get; set; }
       
        public string ShipPhone { get; set; }
      
        public string ShipAddress { get; set; }

        public decimal ShippingCost { get; set; } 
        
        public decimal Total { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }        

        public int? CouponID { get; set; }            
    }
}
