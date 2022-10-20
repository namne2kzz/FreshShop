using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryFilterRequest>>> GetAllCategoryFilter(string languageId);

        Task<ApiResult<PagedResult<CategoryViewModel>>> GetAllCategoryByLanguageId(GetCategoryPagingRequest request);

        Task<ApiResult<List<CategoryViewModel>>> GetAllChildByCategoryId(GetAllChildRequest request);

        Task<ApiResult<int>> Create(CategoryCreateRequest request);

        Task<ApiResult<CategoryViewModel>> GetById(string languageId, int categoryId);

        Task<ApiResult<bool>> Delete(int categoryId);

        Task<ApiResult<bool>> Update(CategoryUpdateRequest request);

        Task<bool> UpdateStatus(int categoryId);
    }
}
