using FreshShop.Utilities;
using FreshShop.ViewModels.Catalog.Cart;
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
    public class CartApiClient : ICartApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<bool>> Create(CartCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/carts/", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public async Task<ApiResult<bool>> CreateOrder(OrderCreateRequest request, List<OrderDetailCreateRequest> requests)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/carts/order", httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) 
            { 
                var orderId= JsonConvert.DeserializeObject<int>(body);
                var result= await CreateOrderDetail(orderId, requests);
                if (result.IsSuccessed) return new ApiSuccessResult<bool>();
                return new ApiErrorResult<bool>(result.Message);
            }                                
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public async Task<ApiResult<bool>> CreateOrderDetail(int orderId, List<OrderDetailCreateRequest> requests)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var json = JsonConvert.SerializeObject(requests);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/carts/orderdetail/{orderId}?orderid={orderId}", httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public async Task<ApiResult<bool>> DeleteCart(Guid userId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);         

            var response = await client.DeleteAsync($"/api/carts/user/{userId}?userId={userId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public async Task<ApiResult<CartViewModel>> GetCartById(int id, string languageId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var response = await client.GetAsync($"/api/carts/cart/{id}?id={id}&languageId={languageId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<CartViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<CartViewModel>>(body);

        }

        public async Task<ApiResult<PagedResult<CartViewModel>>> GetCartByUserId(GetCartPagingRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var response = await client.GetAsync($"/api/carts/user/{request.UserId}?UserId={request.UserId}&LanguageId={request.LanguageId}&Keyword={request.Keyword}&pageIndex={request.PageIndex}&pageSize={request.PageSize}");
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<CartViewModel>>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<PagedResult<CartViewModel>>>(body);
        }

        public async Task<ApiResult<bool>> RemoveItem(int id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var response = await client.DeleteAsync($"/api/carts/{id}?id={id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public async Task<ApiResult<bool>> UpdateCart(CartUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(SystemConstants.BaseAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"/api/carts/", httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }
    }
}
