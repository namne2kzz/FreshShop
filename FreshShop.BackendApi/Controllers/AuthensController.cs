using FreshShop.Business.System.Users;
using FreshShop.ViewModels.System.Users;
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
    public class AuthensController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthensController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.Authenticate(request);
            if(!resultToken.IsSuccessed)
            {
                return BadRequest(resultToken);
            }          
            return Ok(resultToken);
        }

        [HttpPost("client/authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateClient([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.AuthenticateClient(request);
            if (!resultToken.IsSuccessed)
            {
                return BadRequest(resultToken);
            }
            return Ok(resultToken);
        }

        [HttpPost("client/register")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var result = await _userService.RegisterClient(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


    }
}
