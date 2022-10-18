using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Promotion
{
    public class PromotionCreateRequest
    {
        public int ProductId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public decimal Discount { get; set; }
    }
}
