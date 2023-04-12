using FreshShop.Data.EF;
using FreshShop.ViewModels.System.Language;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly FreshShopDbContext _context;

        public LanguageService(FreshShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var languages = await _context.Languages.Select(x => new LanguageViewModel()
            {
                Id=x.Id,
                Name=x.Name
            }).ToListAsync();

            return languages;
        }
    }
}
