using FreshShop.ViewModels.Catalog.Promotion;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Promotions
{
    public interface IPromotionService
    {
        Task<ApiResult<PromotionViewModel>> GetById(int id);

        Task<ApiResult<PromotionViewModel>> GetByProductId(int id);

        Task<int> Create(PromotionCreateRequest request);

        Task<ApiResult<bool>> Update(PromotionUpdateRequest request);

        Task<ApiResult<bool>> Delete(int id);
    }
}
