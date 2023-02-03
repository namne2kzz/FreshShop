using FreshShop.ViewModels.Catalog.Cart;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Cart
{
    public interface ICartService
    {
        Task<ApiResult<PagedResult<CartViewModel>>> ListCartByUser(GetCartPagingRequest request);

        Task<ApiResult<bool>> Create(CartCreateRequest request);

        Task<ApiResult<CartViewModel>> GetCartById(int id, string languageid);

        Task<ApiResult<bool>> UpdateCart(CartUpdateRequest request);

        Task<ApiResult<bool>> RemoveItem(int id);

        Task<ApiResult<bool>> DeleteCart(Guid userId);

        Task<int> CreateOrder(OrderCreateRequest request);

        Task<ApiResult<bool>> CreateOrderDetail(int orderId, List<OrderDetailCreateRequest> requests);
        
    }
}
