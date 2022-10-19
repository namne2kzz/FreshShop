using FreshShop.Data.EF;
using FreshShop.Data.Entities;
using FreshShop.ViewModels.Catalog.Coupon;
using FreshShop.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using FreshShop.Utilities.Exceptions;

namespace FreshShop.Business.Catalog.Coupons
{
    public class CouponService : ICouponService
    {
        private readonly FreshShopDbContext _context;

        public CouponService(FreshShopDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeStatus(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null) throw new FreshShopException("Không tìm thấy mã giảm giá hợp lệ");

            coupon.Status = !coupon.Status;
            await _context.SaveChangesAsync();
            return coupon.Status;
        }

        public async Task<int> Create(CouponCreateRequest request)
        {
            if ((await _context.Coupons.CountAsync(x => x.Code == request.Code)) > 0) return -1;

            var coupon = new Coupon()
            {
                Code = request.Code.ToUpper(),
                Title = request.Title,
                Discount = request.Discount,
                Quantity = request.Quantity,
                FromDate = request.FromDate,
                ExpiredDate = request.ExpiredDate,
                Status = true
            };
            _context.Coupons.Add(coupon);
            if (await _context.SaveChangesAsync() > 0) return coupon.ID;
            return -1;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null) return new ApiErrorResult<bool>("Không tìm thấy mã giảm giá");
            _context.Coupons.Remove(coupon);
            if (await _context.SaveChangesAsync() > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<PagedResult<CouponViewModel>>> GetAllPaging(GetCouponPagingRequest request)
        {
            var coupons = from a in _context.Coupons
                          select new { a };

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                coupons = coupons.Where(x => x.a.Title.Contains(request.Keyword) || x.a.Code.Contains(request.Keyword));
            }
            int totalRow = await coupons.CountAsync();

            var data = await coupons.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new CouponViewModel()
                {
                    Id = x.a.ID,
                    Title = x.a.Title,
                    Code = x.a.Code,
                    Discount = x.a.Discount,
                    Quantity = x.a.Quantity,
                    FromDate = x.a.FromDate,
                    ExpiredDate = x.a.ExpiredDate,
                    Status = x.a.Status
                }).ToListAsync();

            var pageResult = new PagedResult<CouponViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<CouponViewModel>>(pageResult);
        }

        public async Task<ApiResult<CouponViewModel>> GetById(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null) return new ApiErrorResult<CouponViewModel>();

            var couponViewModel = new CouponViewModel()
            {
                Id=coupon.ID,
                Code=coupon.Code,
                Title=coupon.Title,
                Discount=coupon.Discount,
                Quantity=coupon.Quantity,
                FromDate=coupon.FromDate,
                ExpiredDate=coupon.ExpiredDate,
                Status=coupon.Status
            };
            return new ApiSuccessResult<CouponViewModel>(couponViewModel);
        }

        public async Task<ApiResult<CouponViewModel>> GetCouponByCode(string code)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.Code == code);

            if (coupon == null) return new ApiErrorResult<CouponViewModel>();

            var couponViewModel = new CouponViewModel()
            {
                Id = coupon.ID,
                Code = coupon.Code,
                Title = coupon.Title,
                Discount = coupon.Discount,
                Quantity = coupon.Quantity,
                FromDate = coupon.FromDate,
                ExpiredDate = coupon.ExpiredDate,
                Status = coupon.Status
            };
            return new ApiSuccessResult<CouponViewModel>(couponViewModel);

        }

        public async Task<ApiResult<bool>> Update(CouponUpdateRequest request)
        {
            var coupon = await _context.Coupons.FindAsync(request.Id);

            if (coupon == null) return new ApiErrorResult<bool>();

            coupon.Title = request.Title;            
            coupon.Discount = request.Discount;
            coupon.Quantity = request.Quantity;
            coupon.FromDate = request.FromDate;
            coupon.ExpiredDate = request.ExpiredDate;         

            if (await _context.SaveChangesAsync() > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<ApiResult<bool>> UpdateQuantity(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null) return new ApiErrorResult<bool>("Không tìm thấy mã giảm giá");

            coupon.Quantity -= 1;

            if (await _context.SaveChangesAsync() > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }
    }
}
