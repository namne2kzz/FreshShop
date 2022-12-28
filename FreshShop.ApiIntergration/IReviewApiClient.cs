using FreshShop.ViewModels.Catalog.Review;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface IReviewApiClient
    {
        Task<ApiResult<List<ReviewViewModel>>> GetAllReviewByProduct(int productId);

        Task<ApiResult<ReviewViewModel>> GetReviewById(int id);

        Task<int> Create(ReviewCreateRequest request);
    }
}
