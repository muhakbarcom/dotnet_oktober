using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotnet_oktober.Services.AuthService
{
    public interface iAuthService
    {
        Task<ServiceResponse<List<AuthResDto>>> Login(AuthUserDto request);
        Task<ServiceResponse<List<GetUserDto>>> Register(AddUserDto newAuth);
        // Task<ServiceResponse<List<GetUserDto>>> Logout();

    }
}