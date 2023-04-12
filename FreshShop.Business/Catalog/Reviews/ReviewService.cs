using FreshShop.Data.EF;
using FreshShop.ViewModels.Catalog.Review;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FreshShop.Data.Entities;
using FreshShop.ViewModels.Common;

namespace FreshShop.Business.Catalog.Reviews
{
    public class ReviewService : IReviewService
    {

        private readonly FreshShopDbContext _context;

        public ReviewService(FreshShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ReviewCreateRequest request)
        {
            var review = new Review()
            {
                ProductID = request.ProductID,
                UserId = request.UserId,
                Message = request.Message,
                CreatedDate = DateTime.Now,
                Status = true,
                ReplyID = null
            };
            _context.Reviews.Add(review);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return review.ID;
            return -1;
        }

        public async Task<List<ReviewViewModel>> GetAllByProduct(int productId)
        {
            var data = from r in _context.Reviews                         
                          join u in _context.Users
                          on r.UserId equals u.Id
                          select new { r, u };

            var reviews = await data.Where(x => x.r.ProductID == productId).Select(x => new ReviewViewModel()
            {
                ID = x.r.ID,
                UserId = x.r.UserId,
                Username = x.u.UserName,
                ImagePath = x.u.ImagePath,
                ProductID = x.r.ProductID,
                CreatedDate = x.r.CreatedDate,
                Message = x.r.Message,
                Status = x.r.Status
            }).ToListAsync();

            return reviews;
        }

        public async Task<ApiResult<ReviewViewModel>> GetById(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            var user = await _context.Users.FindAsync(review.UserId);
            if (review == null) return new ApiErrorResult<ReviewViewModel>("Không tìm thấy bình luận");

            var reviewViewModel = new ReviewViewModel()
            {
                ID = review.ID,
                CreatedDate = review.CreatedDate,
                ImagePath = user.ImagePath,
                Message = review.Message,
                ProductID = review.ProductID,
                Status = review.Status,
                UserId = review.UserId,
                Username = user.UserName
            };
            return new ApiSuccessResult<ReviewViewModel>(reviewViewModel);
        }
    }
}
