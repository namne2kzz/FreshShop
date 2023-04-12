using FreshShop.Data.Entities;
using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Catalog.ProductImage;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<ApiResult<bool>> Update(ProductUpdateRequest request);

        Task<ApiResult<bool>> Delete(int productId);

        Task<ApiResult<bool>> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateViewCount(int productId);

        Task<bool> UpdateSold(int productId, int quantity);

        Task<bool> UpdateStock(int productId, int quantity);

        Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId);

        Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByLanguageId(GetManageProductPagingRequest request);

        Task<int> AddImage(ProductImageCreateRequest request);

        Task<int> ChangeImageStatus(int imageId);

        Task<ApiResult<bool>> DeleteImage(int imageId);

        Task<List<ProductImageViewModel>> GetListImage(int productId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetManageProductPagingRequest request, string languageId);

        //Binding client data

        Task<List<ProductViewModel>> GetAllSale(string languageId);

        Task<List<ProductViewModel>> GetAllLatest(string languageId);

        Task<List<ProductViewModel>> GetAllBestSeller(string languageId);

        Task<ApiResult<PagedResult<ProductViewModel>>> GetAll(GetPublicProductPagingRequest request);

        Task<List<ProductViewModel>> GetAllRelated(GetRelatedProductRequest request);

      

    }
}
