using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Review
{
    public class ReviewViewModel
    {
        public int ID { get; set; }      

        public Guid UserId { get; set; }

        public String Username { get; set; }

        public String ImagePath { get; set; }

        public int ProductID { get; set; }
       
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
       
    }
}
