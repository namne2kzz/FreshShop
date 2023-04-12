using FreshShop.Data.EF;
using FreshShop.ViewModels.Catalog.Cart;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FreshShop.Utilities;

namespace FreshShop.Business.Catalog.Cart
{
    public class CartService : ICartService
    {
        private readonly FreshShopDbContext _context;

        public CartService(FreshShopDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(CartCreateRequest request)
        {
            var listCartUser = _context.Carts.Where(x => x.UserId == request.UserId).ToList();
            if (listCartUser.Count > 0)
            {
                foreach (var item in listCartUser)
                {
                    if (item.ProductID == request.ProductID)
                    {
                        item.Quantity += 1;                        
                        break;
                    }                   
                }
            }
            else
            {
                var cart = new Data.Entities.Cart()
                {
                    UserId = request.UserId,
                    ProductID = request.ProductID,
                    Quantity = request.Quantity,
                    CreatedDate = DateTime.Now
                };
                _context.Carts.Add(cart);
            }

            var change = await _context.SaveChangesAsync();
            if (change == 0)
            {
                var cart = new Data.Entities.Cart()
                {
                    UserId = request.UserId,
                    ProductID = request.ProductID,
                    Quantity = request.Quantity,
                    CreatedDate = DateTime.Now
                };
                _context.Carts.Add(cart);

            }

            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Thêm vào giỏ thất bại");
        }

        public async Task<int> CreateOrder(OrderCreateRequest request)
        {
            var order = new Order()
            {              
                CreatedDate = request.CreatedDate,
                ShipAddress = request.ShipAddress,
                ShipEmail = request.ShipEmail,
                ShipName = request.ShipName,
                ShipPhone = request.ShipPhone,
                ShippingCost = request.ShippingCost,
                Status = request.Status,
                Total = request.Total,
                UserId = request.UserId,
            };
            if (request.CouponID.Value !=0)
            {
                order.CouponID = request.CouponID.Value;
                var coupon = await _context.Coupons.FindAsync(request.CouponID.Value);
                coupon.Quantity -= coupon.Quantity;
            }
            _context.Orders.Add(order);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return order.ID;
            return -1;           
        }

        public async Task<ApiResult<bool>> CreateOrderDetail(int orderId, List<OrderDetailCreateRequest> requests)
        {
            foreach (var item in requests)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = item.Amount,
                    OrderID = orderId,
                    ProductID = item.ProductID,                   
                    Quantity = item.Quantity
                };
                if (item.PromotionID.HasValue)
                {
                    orderDetail.PromotionID = item.PromotionID.Value;
                }
                _context.OrderDetails.Add(orderDetail);
                var cart = await _context.Carts.FindAsync(item.CartID);
                _context.Carts.Remove(cart);
                var product = await _context.Products.FindAsync(item.ProductID);
                product.Stock -= item.Quantity;
                product.Sold += item.Quantity;
            }
            var resultOD = await _context.SaveChangesAsync();
            if (resultOD > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Thêm chi tiết đơn hàng thất bại");
        }

        public async Task<ApiResult<bool>> DeleteCart(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new ApiErrorResult<bool>("Người dùng không hợp lệ");
            var carts = await _context.Carts.Where(x => x.UserId == userId).ToListAsync();
            if (carts != null)
            {
                foreach (var item in carts)
                {
                    _context.Carts.Remove(item);
                }
            }
            var result = _context.SaveChangesAsync();
            if (result.Result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa giỏ hàng thất bại");

        }

        public async Task<ApiResult<CartViewModel>> GetCartById(int id, string languageId)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return new ApiErrorResult<CartViewModel>("Id không hợp lệ");

            var product = _context.Products.Where(x => x.ID == cart.ProductID).SingleOrDefault();
            var productTranslation = _context.ProductTranslations.Where(x => x.ProductId == cart.ProductID && x.LanguageId == languageId).SingleOrDefault();
            var image = _context.Images.Where(x => x.ProductID == cart.ProductID && x.IsDefault == true).SingleOrDefault();
            var promotion = _context.Promotions.Where(x => x.ProductID == cart.ProductID && x.FromDate<=DateTime.Now && x.ExpiredDate>=DateTime.Now && x.Status==true).SingleOrDefaultAsync();
            var cartViewModel = new CartViewModel()
            {
                ID = cart.ID,
                UserId = cart.UserId,
                ProductID = cart.ProductID,
                CreatedDate = cart.CreatedDate,
                ImagePath = image.ImagePath,
                Price = product.Price,
                Discount= promotion==null?0:promotion.Result.Discount,
                ProductName = productTranslation.Name,
                Quantity = cart.Quantity
            };
            return new ApiSuccessResult<CartViewModel>(cartViewModel);
        }

        public async Task<ApiResult<PagedResult<CartViewModel>>> ListCartByUser(GetCartPagingRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) return new ApiErrorResult<PagedResult<CartViewModel>>("Người dùng không hợp lệ");

            var data = from c in _context.Carts
                       join p in _context.Products
                       on c.ProductID equals p.ID
                       join u in _context.Users
                       on c.UserId equals u.Id
                       where c.UserId == request.UserId
                       join pt in _context.ProductTranslations
                       on c.ProductID equals pt.ProductId
                       where pt.LanguageId == request.LanguageId
                       join i in _context.Images
                       on p.ID equals i.ProductID
                       where i.IsDefault == true       
                       join pr in _context.Promotions.Where(x=>x.FromDate<=DateTime.Now && x.ExpiredDate>=DateTime.Now && x.Status==true)
                       on c.ProductID equals pr.ProductID
                       into temp
                       from last in temp.DefaultIfEmpty()
                       select new { c, p, u, pt, i, last };
           

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                data = data.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            int totalRow = await data.CountAsync();

            var result = await data.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new CartViewModel()
                {
                    ID = x.c.ID,
                    UserId = x.c.UserId,
                    ProductID = x.c.ProductID,
                    CreatedDate = x.c.CreatedDate,
                    ImagePath = x.i.ImagePath,
                    Price = x.p.Price,
                    Discount = x.last==null?0:x.last.Discount,
                    ProductName = x.pt.Name,
                    Quantity = x.c.Quantity
                }).ToListAsync();

            var pageResult = new PagedResult<CartViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<CartViewModel>>(pageResult);

        }

        public async Task<ApiResult<bool>> RemoveItem(int id)
        {
            var item = await _context.Carts.FindAsync(id);
            if (item == null) return new ApiErrorResult<bool>("Không tìm thấy trong giỏ hàng");
            _context.Carts.Remove(item);
            var result = _context.SaveChangesAsync();
            if (result.Result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa sản phẩm thất bại");
        }

        public async Task<ApiResult<bool>> UpdateCart(CartUpdateRequest request)
        {
            var item = await _context.Carts.FindAsync(request.Id);
            if (item == null) return new ApiErrorResult<bool>("Không tìm thấy trong giỏ hàng");
            if (request.Quantity == 0)
            {
                _context.Carts.Remove(item);
            }
            else
            {
                item.Quantity = request.Quantity;
            }

            var result = _context.SaveChangesAsync();
            if (result.Result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật sản phẩm thất bại");
        }
    }
}
