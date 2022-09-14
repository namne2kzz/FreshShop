using FreshShop.Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Common;

namespace FreshShop.Business.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly FreshShopDbContext _context;

        public PublicProductService(FreshShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var query = from a in _context.Products
                        join b in _context.ProductTranslations on a.ID equals b.ProductId
                        join c in _context.Categories on a.CategoryID equals c.ID
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

                }).ToListAsync();          

            return data;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var query = from a in _context.Products
                        join b in _context.ProductTranslations on a.ID equals b.ProductId
                        join c in _context.Categories on a.CategoryID equals c.ID
                        select new { a, b };
            
            if (request.CategoryId.HasValue && request.CategoryId.Value>0)
            {
                query = query.Where(x => x.a.CategoryID==request.CategoryId);
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
    }
}
