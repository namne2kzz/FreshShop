using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
  
    public partial class Address
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [StringLength(255)]
        public string Province { get; set; }

        [Required]
        [StringLength(255)]
        public string District { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string AddressDetail { get; set; }

        public bool IsDefault { get; set; }

        public virtual Customer Customer { get; set; }
    }
}

