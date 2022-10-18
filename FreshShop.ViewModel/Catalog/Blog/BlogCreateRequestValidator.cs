using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Blog
{
    public class BlogCreateRequestValidator : AbstractValidator<BlogCreateRequest>
    {
        public BlogCreateRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề không được rỗng");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung không được rỗng");

            RuleFor(x => x.ThumbnailImage).NotEmpty().WithMessage("Hình ảnh không được rỗng");

        }
    }
}
