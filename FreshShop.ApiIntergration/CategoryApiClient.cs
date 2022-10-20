using FreshShop.Utilities;
using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
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

        public async Task<ApiResult<int>> Create(CategoryCreateRequest request)
        {
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.CategoryName.ToString()), "categoryName");
            requestContent.Add(new StringContent(request.ParentId.ToString()), "parentId");
            requestContent.Add(new StringContent(request.IsShowOnHome.ToString()), "isShowOnHome");          
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(languageId), "languageId");

            var response = await client.PostAsync($"/api/categories/", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int categoryId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"/api/categories/{categoryId}?categoryId={categoryId}");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }   

        public async Task<ApiResult<PagedResult<CategoryViewModel>>> GetAllCategoryByLanguageId(GetCategoryPagingRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories/languageId?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&languageId={request.LanguageId}");

            var body = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<CategoryViewModel>>>(body);
            return categories;
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

        public async Task<ApiResult<List<CategoryViewModel>>> GetAllChildByCategoryId(GetAllChildRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories/{request.LanguageId}/{request.CategoryId}/child?languageId={request.LanguageId}&categoryId={request.CategoryId}");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<CategoryViewModel> categoriesDeserializedObj = (List<CategoryViewModel>)JsonConvert.DeserializeObject(body, typeof(List<CategoryViewModel>));
                return new ApiSuccessResult<List<CategoryViewModel>>(categoriesDeserializedObj);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<CategoryViewModel>>>(body);
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(string languageId, int categoryId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/categories/{languageId}/{categoryId}?languageId={languageId}&categoryId={categoryId}");

            var body = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<CategoryViewModel>>(body);
            return JsonConvert.DeserializeObject < ApiErrorResult<CategoryViewModel>>(body);
        }

        public async Task<ApiResult<bool>> Update(CategoryUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/categories/{request.CategoryId}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<bool> UpdateStatus(int categoryId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));

            var json = JsonConvert.SerializeObject(categoryId);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"/api/categories/{categoryId}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(result);
            }
            return JsonConvert.DeserializeObject<bool>(result);
        }
    }
}
