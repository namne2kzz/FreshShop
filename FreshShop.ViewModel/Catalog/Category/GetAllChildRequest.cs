using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Category
{
    public class GetAllChildRequest 
    {
        public int CategoryId { get; set; }

        public string LanguageId { get; set; }
    }
}
