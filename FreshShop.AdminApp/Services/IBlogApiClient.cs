using FreshShop.ViewModels.Catalog.Blog;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Services
{
    public interface IBlogApiClient
    {      
        Task<ApiResult<PagedResult<BlogViewModel>>> GetAll(GetBlogPagingRequest request);      

        Task<ApiResult<int>> Create(BlogCreateRequest request);

        Task<ApiResult<BlogViewModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> Update(BlogUpdateRequest request);

        Task<bool> UpdateStatus(int id);
    }
}
