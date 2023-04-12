using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.ApiIntergration
{
    public interface IAddressApiClient
    {
        Task<ApiResult<List<GetProvinceRequest>>> GetListProvince();

        Task<ApiResult<List<GetDistrictRequest>>> GetListDistrictByProvince(int provinceId);

        Task<ApiResult<List<AddressViewModel>>> GetAllByUserId(Guid id);

        Task<ApiResult<int>> Create(AddressCreateRequest request);

        Task<ApiResult<AddressViewModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> Update(AddressUpdateRequest request);

        Task<ApiResult<bool>> ChangeDefaultAddress(int id);

        Task<ApiResult<GetProvinceRequest>> GetProvince(int provinceId);

        Task<ApiResult<GetDistrictRequest>> GetDistrict(int districtId);
    }
}
