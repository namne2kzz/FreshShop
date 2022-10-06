using FreshShop.ViewModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryFilterRequest>> GetAllCategoryFilter(string languageId);
    }
}
