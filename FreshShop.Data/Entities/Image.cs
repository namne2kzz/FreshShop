using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
   
    public partial class Image
    {
        public int ID { get; set; }

        public int ProductID { get; set; }
      
        [Required]
        [StringLength(50)]
        public string ImagePath { get; set; }

        public bool IsDefault { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
