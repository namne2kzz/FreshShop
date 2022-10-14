using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Product
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên sản phẩm không được rỗng")
                .MaximumLength(200).WithMessage("Tên sản phẩm không quá 200 kí tự");

            RuleFor(x => x.Unit).NotEmpty().WithMessage("Đơn vị tính không đucợ rỗng")
                .MaximumLength(200).WithMessage("Đơn vị tính không quá 200 kí tự");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá bán không được rỗng").GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");

            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("Giá nhập không được rỗng").GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");

            RuleFor(x => x.Stock).NotEmpty().WithMessage("Số lượng không được rỗng").GreaterThan(0).WithMessage("Số lượng sản phẩm phải lớn hơn 0");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Giá bán không được rỗng").GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");           


        }
    }
}
