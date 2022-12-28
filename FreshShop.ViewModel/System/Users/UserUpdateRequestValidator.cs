using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.System.Users
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được rỗng")
                .MaximumLength(200).WithMessage("Tên không quá 200 kí tự");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ không được rỗng")
                .MaximumLength(200).WithMessage("Họ không quá 200 kí tự");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Dob.Year > (DateTime.Now.Year - 15))
                {
                    context.AddFailure("Năm sing không hợp lệ. Yêu cầu lớn hơn 15 tuổi");
                }
            });

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được rỗng")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$").WithMessage("Email không hợp lệ");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được rỗng")
                .MaximumLength(15).WithMessage("Số điện thoại không quá 15 kí tự").Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$").WithMessage("Số điện thoại không hợp lệ");         


        }
    }
}
