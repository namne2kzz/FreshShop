using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
  
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ID { get; set; }        

        public Guid UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string ShipName { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipEmail { get; set; }

        [Required]
        [StringLength(15)]
        public string ShipPhone { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ShipAddress { get; set; }

        public decimal ShippingCost { get; set; }      

        public decimal Total { get; set; }

        public int? CouponID { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }

        public virtual Coupon Coupon { get; set; }      

        public virtual AppUser AppUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
