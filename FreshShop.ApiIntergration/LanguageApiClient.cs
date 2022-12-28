using FreshShop.Utilities;
using FreshShop.ViewModels.Common;
using FreshShop.ViewModels.System.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public class LanguageApiClient : ILanguageApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageApiClient( IHttpClientFactory httpClientFactory,IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/languages");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<LanguageViewModel> rolesDeserializedObj = (List<LanguageViewModel>)JsonConvert.DeserializeObject(body, typeof(List<LanguageViewModel>));
                return new List<LanguageViewModel>(rolesDeserializedObj);
            }
            return JsonConvert.DeserializeObject<List<LanguageViewModel>>(body);
        }
    }
}
