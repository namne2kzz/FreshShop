using FreshShop.Data.EF;
using FreshShop.ViewModels.Catalog.Category;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly FreshShopDbContext _context;

        public CategoryService(FreshShopDbContext context)
        {
            _context = context;
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
    }
}
