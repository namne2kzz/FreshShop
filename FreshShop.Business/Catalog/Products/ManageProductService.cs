using FreshShop.Data.EF;
using FreshShop.Data.Entities;
using FreshShop.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.Catalog.Product;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using FreshShop.Business.Common;

namespace FreshShop.Business.Catalog.Products
{
    class ManageProductService : IManageProductService
    {
        private readonly FreshShopDbContext _context;
        private readonly IStorageService _storageService;

        public ManageProductService(FreshShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from a in _context.Products
                        join b in _context.ProductTranslations on a.ID equals b.ProductId
                        join c in _context.Categories on a.CategoryID equals c.ID
                        select new { a, b };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.b.Name.Contains(request.Keyword));
            }
            if (request.CategoryId.Count > 0)
            {
                query = query.Where(x => request.CategoryId.Contains(x.a.CategoryID));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {

                    ID = x.a.ID,
                    CategoryID = x.a.CategoryID,
                    LanguageId = x.b.LanguageId,
                    Name = x.b.Name,
                    Unit = x.a.Unit,
                    Price = x.a.Price,
                    ViewCount = x.a.ViewCount,
                    Description = x.b.Description,
                    SeoAlias = x.b.SeoAlias,
                    SeoTitle = x.b.SeoTitle,

                }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };

            return pageResult;
        }

        public async Task<List<Image>> GetListImage(int productId)
        {
            var imageList = _context.Images.Where(x => x.ProductID == productId).ToListAsync();
            return await imageList;
        }

        public async Task<int> AddImage(int productId, List<IFormFile> file)
        {
            foreach(var item in file)
            {
                Image img = new Image();
                img.ProductID = productId;
                img.ImagePath = await this.SaveFile(item);
                img.IsDefault = false;
                img.CreatedDate = DateTime.Now;
                _context.Images.Add(img);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {              
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Unit = request.Unit,
                Stock = request.Stock,
                Sold = 0,
                ViewCount = 0,
                CategoryID = request.CategoryId,
                CreatedDate = DateTime.Now,
                Status=true,
                ProductTranslations=new List<ProductTranslation>() 
                { 
                    new ProductTranslation()
                    {
                        Name=request.Name,
                        Description=request.Description,
                        SeoAlias=request.SeoAlias,
                        SeoTitle=request.SeoTitle,
                        LanguageId=request.LanguageId,
                    }
                }
            };
            if(request.ThumbnailImage != null)
            {
                product.Images = new List<Image>()
                {
                    new Image
                    {
                        ImagePath=await this.SaveFile(request.ThumbnailImage),
                        IsDefault=true,
                        CreatedDate=DateTime.Now,
                    }
                };
            }

            _context.Products.Add(product);
            return await _context.SaveChangesAsync();
           
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new FreshShopException("Cannot find product");

            var thumnailImage = _context.Images.Where(x => x.ProductID == productId);
           foreach(var item in thumnailImage)
            {
                await _storageService.DeleteFileAsync(item.ImagePath);
            }

            _context.Products.Remove(product);
                                
            return await _context.SaveChangesAsync();
        }
            
        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null) throw new FreshShopException("Cannot find produt...");
           
            productTranslation.Name = request.Name;
            productTranslation.Description = request.Description;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoTitle = request.SeoTitle;

            if (request.ThumbnailImage != null)
            {
                var thumnailImage =await _context.Images.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductID == request.Id);
                if(thumnailImage != null)
                {
                    thumnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.Images.Update(thumnailImage);                
                }
               
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ChangeImageStatus(int imageId)
        {
            var imageDefault =await  _context.Images.FirstOrDefaultAsync(x => x.IsDefault == true);
            imageDefault.IsDefault = !imageDefault.IsDefault;

            var image = await _context.Images.FindAsync(imageId);
            image.IsDefault = !image.IsDefault;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteImage(int imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
           _context.Images.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new FreshShopException("Cannot find produt...");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() >0;
        }

        public async Task<bool> UpdateSold(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new FreshShopException("Cannot find produt...");
            product.Sold += quantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new FreshShopException("Cannot find produt...");
            product.Sold += quantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            return await _context.SaveChangesAsync() >0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

       
    }
}
