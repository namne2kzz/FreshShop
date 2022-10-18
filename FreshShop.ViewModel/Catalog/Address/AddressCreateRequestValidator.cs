using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Address
{
    public class AddressCreateRequestValidator : AbstractValidator<AddressCreateRequest>
    {
        public AddressCreateRequestValidator()
        {
            RuleFor(x => x.Detail).NotEmpty().WithMessage("Địa chỉ chi tiết không được rỗng");

        }
    }
}
