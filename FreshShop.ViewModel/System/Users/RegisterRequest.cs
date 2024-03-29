﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FreshShop.ViewModels.System.Users
{
    public class RegisterRequest
    {
        [Display(Name="Tên")]
        public string FirstName { get; set; }

        [Display(Name ="Họ")]
        public string LastName { get; set; }

        [Display(Name = "Hình ảnh")]
        [DataType(DataType.Upload)]
        public IFormFile ThumbnailImage { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
