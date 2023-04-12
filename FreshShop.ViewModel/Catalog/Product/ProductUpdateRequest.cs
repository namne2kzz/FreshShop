using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class ProductUpdateRequest
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Display(Name = "Loại sản phẩm")]
        public int CategoryId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        [Display(Name = "Mã ngôn ngữ")]
        public string LanguageId { set; get; }      

    }
}
