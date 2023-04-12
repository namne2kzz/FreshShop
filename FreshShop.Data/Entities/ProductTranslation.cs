using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreshShop.Data.Entities
{
    public class ProductTranslation
    {
        public int Id { set; get; }

        public int ProductId { set; get; }

        public string Name { set; get; }

        [Column(TypeName = "text")]
        public string Description { set; get; }          

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        public string LanguageId { set; get; }

        public Product Product { get; set; }

        public Language Language { get; set; }
    }
}
