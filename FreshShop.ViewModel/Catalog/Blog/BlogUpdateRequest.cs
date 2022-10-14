using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Blog
{
    public class BlogUpdateRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
       
    }
}
