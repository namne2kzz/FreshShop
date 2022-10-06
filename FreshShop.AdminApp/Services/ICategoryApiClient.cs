using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Services
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryFilterRequest>>> GetAllCategoryFilter(string languageId);
    }
}
