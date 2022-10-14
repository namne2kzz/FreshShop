using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
    
    public partial class Transaction
    {
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        public int ExternalTransactionID { get; set; }

        public decimal Amount { get; set; }

        public decimal Fee { get; set; }

        public bool Result { get; set; }

        [Required]
        [StringLength(255)]
        public string Message { get; set; }

        [Required]
        [StringLength(255)]
        public string Provider { get; set; }

        public bool Status { get; set; }

        public Guid UserId { get; set; }      

        public virtual AppUser AppUser { get; set; }
     
    }
}
