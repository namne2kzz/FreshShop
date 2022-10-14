using FreshShop.Business.System.Roles;
using FreshShop.Data.Entities;
using FreshShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetAllByRole([FromQuery]GetUserPagingByRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _roleService.GetAllPagingByRole(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _roleService.Create(request);
            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _roleService.Delete(id);

            if (result.IsSuccessed) return Ok(result);
            return BadRequest(result);
        }
    }
}
