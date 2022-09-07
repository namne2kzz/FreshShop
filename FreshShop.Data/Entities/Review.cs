using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
   
    public partial class Review
    {
        public int ID { get; set; }

        public int? ReplyID { get; set; }

        public int CustomerID { get; set; }

        public int ProductID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }
    }
}
