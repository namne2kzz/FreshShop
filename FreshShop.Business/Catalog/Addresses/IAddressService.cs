using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreshShop.Business.Catalog.Addresses
{
    public interface IAddressService
    {
        Task<List<AddressViewModel>> GetAllByUserId(Guid id);

        Task<ApiResult<AddressViewModel>> GetById(int id);

        Task<int> Create(AddressCreateRequest request);

        Task<ApiResult<bool>> Update(AddressUpdateRequest request);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> ChangeDefaultAddress(int id);
    }
}
