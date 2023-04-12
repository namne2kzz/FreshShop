using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Review
{
    public class ReviewCreateRequest
    {             

        public Guid UserId { get; set; }

        public int ProductID { get; set; }
       
        public string Message { get; set; }
    
    }
}
