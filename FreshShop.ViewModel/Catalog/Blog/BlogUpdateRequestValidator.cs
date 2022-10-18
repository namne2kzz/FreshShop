using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Blog
{
    public class BlogUpdateRequestValidator : AbstractValidator<BlogUpdateRequest>
    {
        public BlogUpdateRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề không được rỗng");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung không được rỗng");          

        }
    }
}
