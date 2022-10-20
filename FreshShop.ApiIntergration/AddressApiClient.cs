using FreshShop.Utilities;
using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public class AddressApiClient : IAddressApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddressApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<bool>> ChangeDefaultAddress(int id)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(id);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"/api/addresses/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<int>> Create(AddressCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/addresses/", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<int>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<int>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/addresses/{id}?id={id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<AddressViewModel>>> GetAllByUserId(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/addresses?id={id}");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<AddressViewModel> addressesDeserializedObj = (List<AddressViewModel>)JsonConvert.DeserializeObject(body, typeof(List<AddressViewModel>));
                return new ApiSuccessResult<List<AddressViewModel>>(addressesDeserializedObj);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<AddressViewModel>>>(body);
        }

        public async Task<ApiResult<AddressViewModel>> GetById(int id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/addresses/{id}?id={id}");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<AddressViewModel>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<AddressViewModel>>(body);
        }

        public async Task<ApiResult<List<GetDistrictRequest>>> GetDistrict(int provinceId)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://provinces.open-api.vn");
            var response = await client.GetAsync($"/api/p/{provinceId}?depth=2");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                GetProvinceRequest provinceDeserializedObj = JsonConvert.DeserializeObject<GetProvinceRequest>(body);
                return new ApiSuccessResult<List<GetDistrictRequest>>(provinceDeserializedObj.Districts);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<GetDistrictRequest>>>(body);
        }

        public async Task<ApiResult<List<GetProvinceRequest>>> GetProvince()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://provinces.open-api.vn");
            var response = await client.GetAsync($"/api/");

            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<GetProvinceRequest> provinceDeserializedObj = (List<GetProvinceRequest>)JsonConvert.DeserializeObject(body, typeof(List<GetProvinceRequest>));
                return new ApiSuccessResult<List<GetProvinceRequest>>(provinceDeserializedObj);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<GetProvinceRequest>>>(body);
        }

        public async Task<ApiResult<bool>> Update(AddressUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("Token"));

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/addresses/{request.Id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }


    }
}
