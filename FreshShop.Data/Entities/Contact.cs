using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
   
    public partial class Contact
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }       

        [Column(TypeName = "text")]
        [Required]
        public string Message { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
