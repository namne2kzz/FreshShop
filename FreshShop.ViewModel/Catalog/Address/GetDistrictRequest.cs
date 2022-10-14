using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Address
{
    public class GetDistrictRequest
    {
        public string  Name { get; set; }

        public int Code { get; set; }

        public string Codename { get; set; }

        public string Division_type { get; set; }

        public int Province_code { get; set; }
    }  
}
