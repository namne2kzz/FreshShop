using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
   
    public partial class Cart
    {
        public int ID { get; set; }       

        public Guid UserId { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }      

        public virtual Product Product { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
