using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryFilterRequest>> GetAllCategoryFilter(string languageId);

        Task<ApiResult<PagedResult<CategoryViewModel>>> GetAllPagingByLanguageId(GetCategoryPagingRequest request);

        Task<int> Create(CategoryCreateRequest request);

        Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string langauegId);

        Task<ApiResult<bool>> Delete(int categoryId);

        Task<ApiResult<bool>> Update(CategoryUpdateRequest request);

        Task<bool> UpdateStatus(int categoryId);

        Task<List<CategoryViewModel>> GetAllChildByCategoryId(GetAllChildRequest request);
    }
}
