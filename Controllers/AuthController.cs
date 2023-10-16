using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_oktober.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly iUserService _UserService;
        private readonly iAuthService _AuthService;

        public AuthController(iUserService UserService, iAuthService AuthService)
        {
            _UserService = UserService;
            _AuthService = AuthService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(AuthUserDto request)
        {
            return Ok(await _AuthService.Login(request));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(AddUserDto request)
        {
            return Ok(await _AuthService.Register(request));
        }
    }
}