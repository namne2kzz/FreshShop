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

        public Guid UserId { get; set; }
             
        public int ProvinceId { get; set; }
               
        public int DistrictId { get; set; }
       
        public string AddressDetail { get; set; }

        public bool IsDefault { get; set; }      

        public virtual AppUser AppUser { get; set; }
    }
}

