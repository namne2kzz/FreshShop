using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Address
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int ProvinceId { get; set; }

        public string ProvinceName { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        public string Detail { get; set; }

        public bool IsDefault { get; set; }
    }
}
