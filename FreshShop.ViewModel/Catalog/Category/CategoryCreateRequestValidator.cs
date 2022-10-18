using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Catalog.Category
{
    public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Tên danh mục không được rỗng");

            RuleFor(x => x.ThumbnailImage).NotEmpty().WithMessage("Hình ảnh không được rỗng");           

        }
    }
}
