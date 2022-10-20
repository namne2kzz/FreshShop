using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Catalog.ProductImage;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByLanguageId(GetManageProductPagingRequest request);

        Task<ApiResult<ProductViewModel>> GetById(int id, string languageId);

        Task<ApiResult<bool>> Update(ProductUpdateRequest request);

        Task<ApiResult<int>> Create(ProductCreateRequest request);

        Task<ApiResult<bool>> UpdatePrice(int id, decimal newPrice);

        Task<ApiResult<bool>> Delete(int id);



        Task<ApiResult<List<ProductImageViewModel>>> GetListManage(int productId);

        Task<ApiResult<bool>> AddImage(int id, IFormFile thumbnailImage);

        Task<ApiResult<bool>> DeleteImage(int productId, int imageId);
    }
}
