using FreshShop.ViewModels.Catalog.Review;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Reviews
{
    public interface IReviewService
    {
        Task<List<ReviewViewModel>> GetAllByProduct(int productId);

        Task<int> Create(ReviewCreateRequest request);

        Task<ApiResult<ReviewViewModel>> GetById(int id);
    }
}
