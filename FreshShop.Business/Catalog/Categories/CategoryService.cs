using FreshShop.Business.Common;
using FreshShop.Data.EF;
using FreshShop.Data.Entities;
using FreshShop.Utilities.Exceptions;
using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly FreshShopDbContext _context;
        private readonly IStorageService _storageService;

        public CategoryService(FreshShopDbContext context,IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var category = new Category()
            {
                ParentID = request.ParentId != null ? request.ParentId : null,
                ImagePath= await this.SaveFile(request.ThumbnailImage),
                IsShowOnHome=request.IsShowOnHome,               
                CategoryTranslations = new List<CategoryTranslation>()
                {
                    new CategoryTranslation()
                    {
                        Name=request.CategoryName,                       
                        SeoAlias=request.SeoAlias,
                        SeoTitle=request.SeoTitle,
                        LanguageId=request.LanguageId,
                    }
                }
            };          

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.ID;
        }

        public async Task<List<CategoryFilterRequest>> GetAllCategoryFilter(string languageId)
        {
            var categories = from a in _context.Categories
                             join b in _context.CategoryTranslations on a.ID equals b.CategoryId
                             where b.LanguageId == languageId
                             select new { a, b };
            return await categories.Select(x => new CategoryFilterRequest()
            {
                CategoryId = x.a.ID,
                CategoryName = x.b.Name,
            }).ToListAsync();
                                 
        }

        public async Task<ApiResult<PagedResult<CategoryViewModel>>> GetAllPagingByLanguageId(GetCategoryPagingRequest request)
        {
            var categories = from a in _context.Categories
                             join b in _context.CategoryTranslations on a.ID equals b.CategoryId
                             where b.LanguageId == request.LanguageId
                             select new { a, b };
            if(request.Keyword != null)
            {
                categories = categories.Where(x => x.b.Name.Contains(request.Keyword));
            }         

            int totalRow = await categories.CountAsync();

            var data= await categories.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new CategoryViewModel()
            {
                CategoryId = x.a.ID,
                CategoryName = x.b.Name,
                ParentId = x.a.ParentID,
                IsShownHome = x.a.IsShowOnHome,
                ImagePath = x.a.ImagePath,
                LanguageId = x.b.LanguageId,
                SeoAlias = x.b.SeoAlias,
                SeoTitle = x.b.SeoTitle,
            }).ToListAsync();

            var pageResult = new PagedResult<CategoryViewModel>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };

            return new ApiSuccessResult<PagedResult<CategoryViewModel>>(pageResult);
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int categoryId, string langauegId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null) return new ApiErrorResult<CategoryViewModel>("Không tìm thấy danh mục");
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.LanguageId == langauegId);
            var categoryViewModel = new CategoryViewModel()
            {
                CategoryId = category.ID,
                ParentId = category.ParentID,
                IsShownHome = category.IsShowOnHome,
                ImagePath = category.ImagePath,
                CategoryName = categoryTranslation != null ? categoryTranslation.Name : null,
                SeoAlias = categoryTranslation != null ? categoryTranslation.SeoAlias : null,
                SeoTitle = categoryTranslation != null ? categoryTranslation.SeoTitle : null,
                LanguageId = categoryTranslation != null ? categoryTranslation.LanguageId : null,
            };

            return new ApiSuccessResult<CategoryViewModel>(categoryViewModel);


        }

        public async Task<ApiResult<bool>> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null) return new ApiErrorResult<bool>("Không tìm thấy danh mục");
            var categoryTranslation = await _context.CategoryTranslations.Where(x => x.CategoryId == categoryId).ToListAsync();
            foreach(var item in categoryTranslation)
            {
                _context.CategoryTranslations.Remove(item);
            }

            _context.Categories.Remove(category);
            await _storageService.DeleteFileAsync(category.ImagePath);
            var result = await _context.SaveChangesAsync();

            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>();
        }

        public async Task<ApiResult<bool>> Update(CategoryUpdateRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null) return new ApiErrorResult<bool>("Không tìm thấy danh mục");
            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId && x.LanguageId == request.LanguageId);

            category.ParentID = request.ParentId;
            categoryTranslation.Name = request.CategoryName;
            categoryTranslation.SeoAlias = request.SeoAlias;
            categoryTranslation.SeoTitle = request.SeoTitle;

            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<List<CategoryViewModel>> GetAllChildByCategoryId(GetAllChildRequest request)
        {
            var categories = from a in _context.Categories
                             join b in _context.CategoryTranslations on a.ID equals b.CategoryId
                             where b.LanguageId == request.LanguageId
                             where a.ParentID==request.CategoryId
                             select new { a, b };          

            int totalRow = await categories.CountAsync();

            var data = await categories.Select(x => new CategoryViewModel()
            {
                CategoryId = x.a.ID,
                CategoryName = x.b.Name,
                ParentId = x.a.ParentID,
                IsShownHome = x.a.IsShowOnHome,
                ImagePath = x.a.ImagePath,
                LanguageId = x.b.LanguageId,
                SeoAlias = x.b.SeoAlias,
                SeoTitle = x.b.SeoTitle,
            }).ToListAsync();

            return data;
        }

        public async Task<bool> UpdateStatus(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null) throw new FreshShopException("Không tìm thấy danh mục");

            category.IsShowOnHome = !category.IsShowOnHome;
            await _context.SaveChangesAsync();
            return category.IsShowOnHome;
        }
    }
}
