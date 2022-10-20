using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration 
{ 
    public interface ILanguageApiClient
    {
        Task<List<LanguageViewModel>> GetAll();
    }
}
