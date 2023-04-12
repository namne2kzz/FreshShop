using FreshShop.ViewModels.System.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.System.Languages
{
    public interface ILanguageService
    {
        public Task<List<LanguageViewModel>> GetAll();
    }
}
