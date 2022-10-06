using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Services
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<CategoryFilterRequest>>> GetAllCategoryFilter(string languageId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories?languageId={languageId}");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<CategoryFilterRequest> categoriesDeserializedObj = (List<CategoryFilterRequest>)JsonConvert.DeserializeObject(body, typeof(List<CategoryFilterRequest>));
                return new ApiSuccessResult<List<CategoryFilterRequest>>(categoriesDeserializedObj);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<CategoryFilterRequest>>>(body);
        }
    }
}
