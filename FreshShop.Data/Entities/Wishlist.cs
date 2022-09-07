using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
   
    public partial class Wishlist
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public int ProductID { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }
    }
}
