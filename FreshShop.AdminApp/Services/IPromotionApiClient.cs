using FreshShop.ViewModels.Catalog.Promotion;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Services
{
    public interface IPromotionApiClient
    {
        Task<int> Create(PromotionCreateRequest request);

        Task<ApiResult<PromotionViewModel>> GetById(int id);

        Task<ApiResult<PromotionViewModel>> GetByProduct(int id);

        Task<ApiResult<bool>> Update(PromotionUpdateRequest request);

        Task<ApiResult<bool>> Delete(int id);
    }
}
