using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Entities
{
    public class Language
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public virtual ICollection<ProductTranslation> ProductTranslations { get; set; }

        public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
