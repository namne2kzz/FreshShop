using FreshShop.Utilities;
using FreshShop.ViewModels.Catalog.Review;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public class ReviewApiClient : IReviewApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public ReviewApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<int> Create(ReviewCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Reviews/", httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<int>(body);
            }
            return JsonConvert.DeserializeObject<int>(body);
        }

        public async Task<ApiResult<List<ReviewViewModel>>> GetAllReviewByProduct(int productId)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);

            var response = await client.GetAsync($"/api/reviews/{productId}?productId={productId}");
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                List<ReviewViewModel> reviewsDeserializeObj = (List<ReviewViewModel>)JsonConvert.DeserializeObject(body,typeof(List<ReviewViewModel>));
                return new ApiSuccessResult<List<ReviewViewModel>>(reviewsDeserializeObj);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<ReviewViewModel>>>(body);
        }

        public Task<ApiResult<ReviewViewModel>> GetReviewById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
