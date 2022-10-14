using FreshShop.ViewModels.Catalog.Blog;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Blogs
{
    public interface IBlogService
    {
        Task<ApiResult<PagedResult<BlogViewModel>>> GetAllPaging(GetBlogPagingRequest request);       

        Task<ApiResult<BlogViewModel>> GetById(int id);

        Task<int> Create(BlogCreateRequest request);

        Task<ApiResult<bool>> Update(BlogUpdateRequest request);

        Task<ApiResult<bool>> Delete(int id);

        Task<bool> ChangeStatus(int id);
    }
}
