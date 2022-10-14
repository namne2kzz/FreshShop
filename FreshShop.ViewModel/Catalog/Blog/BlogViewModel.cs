using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Blog
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
    }
}
