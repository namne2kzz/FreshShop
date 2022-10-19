using FreshShop.ViewModels.Catalog.Coupon;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Coupons
{
    public interface ICouponService
    {
        Task<ApiResult<PagedResult<CouponViewModel>>> GetAllPaging(GetCouponPagingRequest request);

        Task<int> Create(CouponCreateRequest request);

        Task<ApiResult<bool>> Update(CouponUpdateRequest request);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<CouponViewModel>> GetById(int id);

        Task<ApiResult<CouponViewModel>> GetCouponByCode(string code);

        Task<ApiResult<bool>> UpdateQuantity(int id);

        Task<bool> ChangeStatus(int id);
    }
}
