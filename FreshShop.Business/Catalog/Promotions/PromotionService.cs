using FreshShop.Data.EF;
using FreshShop.ViewModels.Catalog.Promotion;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreshShop.Business.Catalog.Promotions
{
    public class PromotionService : IPromotionService
    {
        private readonly FreshShopDbContext _context;

        public PromotionService(FreshShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(PromotionCreateRequest request)
        {
            if (_context.Promotions.Count(x => x.ProductID == request.ProductId && x.Status == true) > 0) return -1;

            var promotion = new Promotion()
            {
                ProductID = request.ProductId,
                FromDate = request.FromDate,
                ExpiredDate = request.ExpiredDate,
                Discount = request.Discount,
                Status = true
            };
            _context.Promotions.Add(promotion);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return promotion.ID;
            return -1;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);

            if (promotion == null) return new ApiErrorResult<bool>("Không tìm thấy kết quả hợp lệ");

            _context.Promotions.Remove(promotion);
            if (await _context.SaveChangesAsync() > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<PromotionViewModel>> GetById(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);

            if (promotion == null) return new ApiErrorResult<PromotionViewModel>("Không tìm thấy");

            var promotionViewModel = new PromotionViewModel()
            {
                Id = promotion.ID,
                ProductId = promotion.ProductID,
                FromDate = promotion.FromDate,
                ExpiredDate = promotion.ExpiredDate,
                Discount = promotion.Discount,
                Status = promotion.Status
            };
            return new ApiSuccessResult<PromotionViewModel>(promotionViewModel);
        }

        public async Task<ApiResult<PromotionViewModel>> GetByProductId(int id)
        {
            var promotion = await _context.Promotions.FirstOrDefaultAsync(x => x.ProductID == id);
            if (promotion == null) return new ApiErrorResult<PromotionViewModel>("Không tìm thấy kết quả hợp lệ");

            var promotionViewModel = new PromotionViewModel()
            {
                Id = promotion.ID,
                ProductId = promotion.ProductID,
                FromDate = promotion.FromDate,
                ExpiredDate = promotion.ExpiredDate,
                Discount = promotion.Discount,
                Status = promotion.Status
            };
            return new ApiSuccessResult<PromotionViewModel>(promotionViewModel);

        }

        public async Task<ApiResult<bool>> Update(PromotionUpdateRequest request)
        {
            var promotion = await _context.Promotions.FindAsync(request.Id);

            if (promotion == null) return new ApiErrorResult<bool>("Không tìm thấy kết quả hợp lệ");

            promotion.FromDate = request.FromDate;
            promotion.ExpiredDate = request.ExpiredDate;
            promotion.Discount = request.Discount;
            promotion.Status = request.Status;
            if (await _context.SaveChangesAsync() > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }
    }
}
