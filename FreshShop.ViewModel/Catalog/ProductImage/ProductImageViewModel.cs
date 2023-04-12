using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FreshShop.ViewModels.Catalog.ProductImage
{
    public class ProductImageViewModel
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "Mã hình ảnh")]
        public int ProductImageId { get; set; }

        [Display(Name = "Tên File ảnh")]
        public string ImagePath { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsDefault { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }
    }
}
