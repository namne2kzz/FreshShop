using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Address
{
    public class AddressCreateRequest
    {
        public Guid UserId { get; set; }

        public int ProvinceId { get; set; }

        public int Districtd { get; set; }

        public string Detail { get; set; }
       
    }
}
