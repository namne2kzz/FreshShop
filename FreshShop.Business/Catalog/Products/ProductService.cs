﻿using FreshShop.Data.EF;
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
using FreshShop.ViewModels.Catalog.ProductImage;

namespace FreshShop.Business.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly FreshShopDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(FreshShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByLanguageId(GetManageProductPagingRequest request)
        {
            var query = from a in _context.Products
                        join b in _context.ProductTranslations on a.ID equals b.ProductId
                        join c in _context.Categories on a.CategoryID equals c.ID
                        join d in _context.CategoryTranslations on a.CategoryID equals d.CategoryId
                        join e in _context.Images on a.ID equals e.ProductID
                        where e.IsDefault == true
                        where b.LanguageId == request.LanguageId && d.LanguageId == request.LanguageId
                        select new { a, b, c, d, e };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.b.Name.Contains(request.Keyword) || x.b.Description.Contains(request.Keyword) || x.d.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(x => x.d.CategoryId == request.CategoryId);
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {

                    ID = x.a.ID,
                    CategoryID = x.a.CategoryID,
                    CategoryName = x.d.Name,
                    LanguageId = x.b.LanguageId,
                    Name = x.b.Name,
                    Unit = x.a.Unit,
                    Price = x.a.Price,
                    OriginalPrice = x.a.OriginalPrice,
                    Stock = x.a.Stock,
                    Sold = x.a.Sold,
                    ViewCount = x.a.ViewCount,
                    Description = x.b.Description,
                    SeoAlias = x.b.SeoAlias,
                    SeoTitle = x.b.SeoTitle,
                    ImagePath = x.e.ImagePath

                }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<ProductViewModel>>(pageResult);
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var imageList = _context.Images.Where(x => x.ProductID == productId).Select(x => new ProductImageViewModel()
            {
                ProductId = x.ProductID,
                ProductImageId = x.ID,
                IsDefault = x.IsDefault,
                CreatedDate = x.CreatedDate,
                ImagePath = x.ImagePath,
            }).ToListAsync();
            return await imageList;

        }

        public async Task<int> AddImage(ProductImageCreateRequest request)
        {

            var img = new Image();
            img.ProductID = request.ProductId;
            img.IsDefault = false;
            img.CreatedDate = DateTime.Now;
            if (request.ThumbnailImage != null)
            {
                img.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            _context.Images.Add(img);

            await _context.SaveChangesAsync();
            return img.ID;
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
                Status = true,
                ProductTranslations = new List<ProductTranslation>()
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
            if (request.ThumbnailImage != null)
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
            await _context.SaveChangesAsync();
            return product.ID;


        }

        public async Task<ApiResult<bool>> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");

            var productTranslation = await _context.ProductTranslations.Where(x => x.ProductId == productId).ToListAsync();
            foreach (var item in productTranslation)
            {
                _context.ProductTranslations.Remove(item);
            }

            var thumnailImage = _context.Images.Where(x => x.ProductID == productId);
            foreach (var item in thumnailImage)
            {
                await _storageService.DeleteFileAsync(item.ImagePath);
            }

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa sản phẩm không thành công");
        }

        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null) throw new FreshShopException("Cannot find produt...");

            product.CategoryID = request.CategoryId;
            productTranslation.Name = request.Name;
            productTranslation.Description = request.Description;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoTitle = request.SeoTitle;


            //if (request.ThumbnailImage != null)
            //{
            //    var thumnailImage = await _context.Images.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductID == request.Id);
            //    if (thumnailImage != null)
            //    {
            //        thumnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
            //        _context.Images.Update(thumnailImage);
            //    }

            //}

            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<int> ChangeImageStatus(int imageId)
        {
            var imageDefault = await _context.Images.FirstOrDefaultAsync(x => x.IsDefault == true);
            imageDefault.IsDefault = !imageDefault.IsDefault;

            var image = await _context.Images.FindAsync(imageId);
            image.IsDefault = !image.IsDefault;

            return await _context.SaveChangesAsync();
        }

        public async Task<ApiResult<bool>> DeleteImage(int imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if (image == null) throw new FreshShopException("Cannot find image");
            if (image.IsDefault) return new ApiErrorResult<bool>("Xóa thất bại");

            _context.Images.Remove(image);
            await _storageService.DeleteFileAsync(image.ImagePath);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa thất bại");

        }

        public async Task<ApiResult<bool>> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new FreshShopException("Cannot find produt...");
            product.Price = newPrice;
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");

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
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.LanguageId == languageId && x.CategoryId == product.CategoryID);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.LanguageId == languageId && x.ProductId == productId);
            var category = await _context.Categories.FindAsync(product.CategoryID);
            var promotion = await _context.Promotions.FirstOrDefaultAsync(x => x.ProductID == productId && x.FromDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now && x.Status==true);

            var productViewModel = new ProductViewModel()
            {
                ID = product.ID,
                CategoryID = category.ID,
                CategoryName = categoryTranslation.Name,
                Name = productTranslation != null ? productTranslation.Name : null,
                Description = productTranslation != null ? productTranslation.Description : null,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                Unit = product.Unit,
                Stock = product.Stock,
                Sold = product.Sold,
                ViewCount = product.ViewCount,
                CreatedDate = product.CreatedDate,
                LanguageId = productTranslation.LanguageId,
                Discount = promotion != null ? promotion.Discount : 0,
            };
            return new ApiSuccessResult<ProductViewModel>(productViewModel);


        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.Images.FindAsync(imageId);

            if (image == null) throw new FreshShopException("Cannot find image");

            var productImageViewModel = new ProductImageViewModel()
            {
                ProductId = image.ProductID,
                ProductImageId = image.ID,
                IsDefault = image.IsDefault,
                CreatedDate = image.CreatedDate,
                ImagePath = image.ImagePath,
            };
            return productImageViewModel;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetManageProductPagingRequest request, string languageId)
        {
            var query = from a in _context.Products
                        join b in _context.ProductTranslations on a.ID equals b.ProductId
                        join c in _context.Categories on a.CategoryID equals c.ID
                        where b.LanguageId == languageId
                        select new { a, b };

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.a.CategoryID == request.CategoryId);
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
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return pageResult;
        }

        //Binding client data

        public async Task<List<ProductViewModel>> GetAllSale(string languageId)
        {
            var query = from a in _context.Promotions.Where(x=>x.FromDate<= DateTime.Now && DateTime.Now<=x.ExpiredDate)
                       join b in _context.Products on a.ProductID equals b.ID
                       join c in _context.ProductTranslations on a.ProductID equals c.ProductId
                       where c.LanguageId == languageId
                       select new { a, b, c };

            var data = query.Select(x => new ProductViewModel()
            {
                ID = x.a.ID,
                CategoryID = x.b.CategoryID,
                LanguageId = x.c.LanguageId,
                Name = x.c.Name,
                Unit = x.b.Unit,
                Price = x.b.Price,
                Discount = x.a.Discount,
                ViewCount = x.b.ViewCount,
                Description = x.c.Description,
                SeoAlias = x.c.SeoAlias,
                SeoTitle = x.c.SeoTitle,
                ImagePath = x.b.Images.Where(i => i.IsDefault == true).SingleOrDefault().ImagePath,
            }).ToList();

            return data;
        }

        public async Task<List<ProductViewModel>> GetAllLatest(string languageId)
        {
            var query = from a in _context.Products.OrderByDescending(x=>x.CreatedDate)
                        join b in _context.ProductTranslations on a.ID equals b.ProductId                       
                        where b.LanguageId == languageId
                        select new { a, b };                     

            var data = await query
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
                    ImagePath = x.a.Images.Where(i => i.IsDefault == true).SingleOrDefault().ImagePath,
                }).ToListAsync();
            return data;
    }

        public async Task<List<ProductViewModel>> GetAllBestSeller(string languageId)
        {
            var query = from a in _context.Products.OrderByDescending(x => x.Sold)
                        join b in _context.ProductTranslations on a.ID equals b.ProductId                        
                        where b.LanguageId == languageId
                        select new { a, b };

            var data = await query
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
                    ImagePath = x.a.Images.Where(i => i.IsDefault == true).SingleOrDefault().ImagePath,
                }).ToListAsync();
            return data;
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAll(GetPublicProductPagingRequest request)
        {
            var query = from a in _context.Products
                        join b in _context.ProductTranslations
                        on a.ID equals b.ProductId
                        where b.LanguageId == request.LanguageId
                        join c in _context.Categories
                        on a.CategoryID equals c.ID
                        join d in _context.CategoryTranslations
                        on a.CategoryID equals d.CategoryId
                        where d.LanguageId == request.LanguageId
                        join e in _context.Images
                        on a.ID equals e.ProductID
                        where e.IsDefault==true
                        join f in _context.Promotions.Where(x => x.FromDate <= DateTime.Now && DateTime.Now <= x.ExpiredDate)
                        on a.ID equals f.ProductID into temp                    
                        from last in temp.DefaultIfEmpty()
                        select new { a, b, c, d, e, last };

            if(request.CategoryId !=null && request.CategoryId != 0)
            {
                query = query.Where(x => x.a.CategoryID == request.CategoryId);
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.b.Name.Contains(request.Keyword) || x.b.Description.Contains(request.Keyword));
            }
            if (request.SortId != null && request.SortId != 0)
            {
                if (request.SortId == 1)
                {
                    query = query.OrderByDescending(x => x.a.Price);
                }
                else if (request.SortId == 2)
                {
                    query = query.OrderBy(x => x.a.Price);
                }
                else if (request.SortId == 3)
                {
                    query = query.OrderByDescending(x => x.a.CreatedDate);
                }
                else if (request.SortId == 4)
                {
                    query = query.Where(x => x.last != null);
                }
                else if(request.SortId==5)
                {
                    query = query.OrderByDescending(x => x.a.Sold);
                }
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    ID = x.a.ID,
                    CategoryID = x.a.CategoryID,
                    CategoryName = x.d.Name,
                    LanguageId = x.b.LanguageId,
                    Name = x.b.Name,
                    Unit = x.a.Unit,
                    Price = x.a.Price,
                    OriginalPrice = x.a.OriginalPrice,
                    Stock = x.a.Stock,
                    Sold = x.a.Sold,
                    ViewCount = x.a.ViewCount,
                    Description = x.b.Description,
                    SeoAlias = x.b.SeoAlias,
                    SeoTitle = x.b.SeoTitle,
                    ImagePath = x.e.ImagePath,
                    CreatedDate=x.a.CreatedDate,
                    Discount=x.last==null?0:x.last.Discount

                }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<ProductViewModel>>(pageResult);
        }

        public async Task<List<ProductViewModel>> GetAllRelated(GetRelatedProductRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);

            var query = from a in _context.Products.Where(x=>x.CategoryID==product.CategoryID && x.ID !=product.ID)
                        join b in _context.ProductTranslations on a.ID equals b.ProductId
                        where b.LanguageId == request.LanguageId
                        select new { a, b };

            var data = await query
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
                    ImagePath = x.a.Images.Where(i => i.IsDefault == true).SingleOrDefault().ImagePath,
                }).ToListAsync();
            return data;
        }
    }
}
