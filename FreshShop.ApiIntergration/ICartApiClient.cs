using FreshShop.ViewModels.Catalog.Cart;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface ICartApiClient
    {
        Task<ApiResult<PagedResult<CartViewModel>>> GetCartByUserId(GetCartPagingRequest request);

        Task<ApiResult<bool>> Create(CartCreateRequest request);

        Task<ApiResult<CartViewModel>> GetCartById(int id, string languageId);

        Task<ApiResult<bool>> UpdateCart(CartUpdateRequest request);

        Task<ApiResult<bool>> RemoveItem(int id);

        Task<ApiResult<bool>> DeleteCart(Guid userId);

        Task<ApiResult<bool>> CreateOrder(OrderCreateRequest request, List<OrderDetailCreateRequest> requests);

        Task<ApiResult<bool>> CreateOrderDetail(int orderId, List<OrderDetailCreateRequest> requests);                
                                                     
    }
}
