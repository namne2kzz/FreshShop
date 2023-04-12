using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class ProductCreateRequest
    {
        [Display(Name = "Mã danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string Unit { get; set; }

        [Display(Name = "Giá nhập")]
        public decimal OriginalPrice { get; set; }

        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Số lượng kho")]
        public int Stock { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }

        [Display(Name = "Mã ngôn ngữ")]
        public string LanguageId { set; get; }

        [Display(Name = "Hình ảnh")]
        public IFormFile ThumbnailImage { get; set; }

    }
}
