using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_oktober.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContextAsync(context, token);
            }

            await _next(context);
        }

        private async Task AttachUserToContextAsync(HttpContext context, string token)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            // serviceResponse
            if (string.IsNullOrWhiteSpace(token))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Token is required";
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(serviceResponse);
                await context.Response.CompleteAsync();
            }


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!principal.Identity.IsAuthenticated) // if token is not valid
                {
                    // return error
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Token is not valid!";
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(serviceResponse);
                    await context.Response.CompleteAsync();
                }
                else
                {
                    context.User = principal;

                    context.Items["USERNAME"] = principal.Identity.Name;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Token is not valid!";
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(serviceResponse);
                await context.Response.CompleteAsync();
            }
        }
    }
}