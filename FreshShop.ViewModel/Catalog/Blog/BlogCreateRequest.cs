using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Blog
{
    public class BlogCreateRequest
    {
       
        public string Title { get; set; }

        public string Content { get; set; }

        public IFormFile ThumbnailImage { get; set; }      
      
    }
}
