using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.AdminApp.Services
{
    public interface IAddressApiClient
    {
        Task<ApiResult<List<GetProvinceRequest>>> GetProvince();

        Task<ApiResult<List<GetDistrictRequest>>> GetDistrict(int provinceId);

        Task<ApiResult<List<AddressViewModel>>> GetAllByUserId(Guid id);

        Task<ApiResult<int>> Create(AddressCreateRequest request);

        Task<ApiResult<AddressViewModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> Update(AddressUpdateRequest request);

        Task<ApiResult<bool>> ChangeDefaultAddress(int id);
    }
}
