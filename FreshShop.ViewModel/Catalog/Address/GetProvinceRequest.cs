using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Address
{
    public class GetProvinceRequest
    {
        public string Name { get; set; }

        public int Code { get; set; }

        public string Division_type { get; set; }

        public string Codename { get; set; }

        public int Phone_code { get; set; }    
        
        public List<GetDistrictRequest> Districts { get; set; }
      
    }
}
