using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Promotion
{
    public class PromotionUpdateRequestValidator : AbstractValidator<PromotionUpdateRequest>
    {
        public PromotionUpdateRequestValidator()
        {
            RuleFor(x => x.FromDate).NotEmpty().WithMessage("Ngày bắt đầu không được rỗng");

            RuleFor(x => x.ExpiredDate).NotEmpty().WithMessage("Ngày hết hạn không được rỗng").GreaterThanOrEqualTo(x => x.FromDate).WithMessage("Ngày hết hạn phải lớn hơn ngày bắt đầu");
               
            RuleFor(x => x.Discount).NotEmpty().WithMessage("Giảm giá không được rỗng").GreaterThan(0).WithMessage("Giảm giá phải lớn hơn 0");         

        }
    }
}
