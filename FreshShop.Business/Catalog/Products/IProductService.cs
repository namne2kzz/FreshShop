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

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateViewCount(int productId);

        Task<bool> UpdateSold(int productId, int quantity);

        Task<bool> UpdateStock(int productId, int quantity);

        Task<ProductViewModel> GetById(int productId, string languageId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> ChangeImageStatus(int imageId);

        Task<int> DeleteImage(int imageId);

        Task<List<ProductImageViewModel>> GetListImage(int productId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        public Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request, string languageId);
    }
}
