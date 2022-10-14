using FreshShop.Business.Catalog.Addresses;
using FreshShop.ViewModels.Catalog.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUserId([FromQuery] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var addresses = await _addressService.GetAllByUserId(id);
            if (addresses == null) return BadRequest(addresses);
            return Ok(addresses);          
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var address = await _addressService.GetById(id);
            if (address.IsSuccessed) return Ok(address);
            return BadRequest(address);

        }

        [HttpPost]      
        public async Task<IActionResult> Create([FromBody] AddressCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var addressId = await _addressService.Create(request);
            if (addressId == 0)
            {
                return BadRequest(addressId);
            }
            var address = await _addressService.GetById(addressId);

            return CreatedAtAction(nameof(GetById), new { id = addressId }, address.ResultObj);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _addressService.Delete(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddressUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addressService.Update(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _addressService.ChangeDefaultAddress(id);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

    }
}
