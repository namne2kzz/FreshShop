using FreshShop.Data.EF;
using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FreshShop.Data.Entities;
using Microsoft.AspNetCore.Identity;
using FreshShop.Utilities.Exceptions;

namespace FreshShop.Business.Catalog.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly FreshShopDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AddressService(FreshShopDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApiResult<bool>> ChangeDefaultAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return new ApiErrorResult<bool>("Địa chỉ không hợp lệ");

            if (address.IsDefault) return new ApiErrorResult<bool>("Không đươc thao tác với địa chỉ mặc định");
           
            address.IsDefault = true;

            var defaultAddress = await _context.Addresses.FirstOrDefaultAsync(x => x.UserId == address.UserId && x.IsDefault == true);
            defaultAddress.IsDefault = false;
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Thao tác thất bại");
        }

        public async Task<int> Create(AddressCreateRequest request)
        {
            int countAddress = await _context.Addresses.CountAsync(x => x.UserId == request.UserId);

            var address = new Address()
            {
                UserId = request.UserId,
                ProvinceId = request.ProvinceId,
                DistrictId = request.Districtd,
                AddressDetail = request.Detail,
                IsDefault = countAddress == 0 ? true : false

            };
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address.ID;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return new ApiErrorResult<bool>("Không tìm thấy địa chỉ hợp lệ");
            if (address.IsDefault) return new ApiErrorResult<bool>("Xóa thất bại");
            _context.Addresses.Remove(address);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<List<AddressViewModel>> GetAllByUserId(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            var addresses = from a in _context.Addresses
                            where a.UserId == id
                            select new AddressViewModel()
                            {
                                Id = a.ID,
                                UserId = a.UserId,
                                ProvinceId = a.ProvinceId,
                                DistrictId = a.DistrictId,
                                Detail = a.AddressDetail,
                                IsDefault = a.IsDefault
                            };
            return await addresses.ToListAsync();           
        }

        public async Task<ApiResult<AddressViewModel>> GetById(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return new ApiErrorResult<AddressViewModel>("Không tìm thấy địa chỉ hợp lệ");
            var addressViewModel = new AddressViewModel()
            {
                Id = address.ID,
                UserId = address.UserId,
                ProvinceId = address.ProvinceId,
                DistrictId = address.DistrictId,
                Detail = address.AddressDetail,
                IsDefault = address.IsDefault
            };

            return new ApiSuccessResult<AddressViewModel>(addressViewModel);
        }

        public async Task<ApiResult<bool>> Update(AddressUpdateRequest request)
        {
            var address = await _context.Addresses.FindAsync(request.Id);
            if (address == null) return new ApiErrorResult<bool>("Không tìm thấy địa chỉ hợp lệ");
            address.ProvinceId = request.ProvinceId;
            address.DistrictId = request.Districtd;
            address.AddressDetail = request.Detail;
            var result = await _context.SaveChangesAsync();
            if (result > 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }
    }
}
