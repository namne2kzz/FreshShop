using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Address
{
    public class AddressUpdateRequestValidator : AbstractValidator<AddressUpdateRequest>
    {
        public AddressUpdateRequestValidator()
        {
            RuleFor(x => x.Detail).NotEmpty().WithMessage("Địa chỉ chi tiết không được rỗng");

        }
    }
}
