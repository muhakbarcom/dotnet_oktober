global using AutoMapper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;


namespace dotnet_oktober.Services.AuthService
{
    public class AuthService : iAuthService
    {
        private readonly DataContex _contex;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public AuthService(IMapper mapper, DataContex contex, IConfiguration configuration)
        {
            _contex = contex;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> Register(AddUserDto newAuth)
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();

            try
            {
                var Auth = _mapper.Map<User>(newAuth);

                // hash password
                Auth.PASSWORD = BCrypt.Net.BCrypt.HashPassword(Auth.PASSWORD, "akbarSecret", true, BCrypt.Net.HashType.SHA256);

                // Menambah karakter baru ke DbContext
                _contex.Users.Add(Auth);
                await _contex.SaveChangesAsync();

                // Mengambil daftar karakter yang sudah ada dari database menggunakan LINQ
                var Users = await _contex.Users
                    .Select(c => _mapper.Map<GetUserDto>(c))
                    .ToListAsync();

                serviceResponse.Data = Users;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

                if (ex.InnerException != null)
                {
                    // Jika terdapat inner exception, Anda bisa mengakses pesan kesalahan lebih rinci dari inner exception.
                    string innerErrorMessage = ex.InnerException.Message;
                    // Lakukan sesuatu dengan innerErrorMessage, misalnya, catat pesan kesalahan lebih lanjut.
                    serviceResponse.Message = innerErrorMessage;
                }

            }

            return serviceResponse;
        }

        //login
        public async Task<ServiceResponse<List<AuthResDto>>> Login(AuthUserDto newAuth)
        {
            var serviceResponse = new ServiceResponse<List<AuthResDto>>();

            try
            {
                // cek apakah ada username di database
                var user = await _contex.Users.FirstOrDefaultAsync(c => c.USERNAME == newAuth.USERNAME);

                if (user == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Username tidak ditemukan";
                    return serviceResponse;
                }

                // cek apakah password benar
                if (!BCrypt.Net.BCrypt.Verify(newAuth.PASSWORD, user.PASSWORD))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Password salah";
                    return serviceResponse;
                }

                string token = CreateToken(user);

                serviceResponse.Data = await _contex.Users
                    .Select(c => _mapper.Map<AuthResDto>(c))
                    .ToListAsync();

                serviceResponse.Data[0].TOKEN = token;


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

                if (ex.InnerException != null)
                {
                    // Jika terdapat inner exception, Anda bisa mengakses pesan kesalahan lebih rinci dari inner exception.
                    string innerErrorMessage = ex.InnerException.Message;
                    // Lakukan sesuatu dengan innerErrorMessage, misalnya, catat pesan kesalahan lebih lanjut.
                    serviceResponse.Message = innerErrorMessage;
                }

            }

            return serviceResponse;
        }

        //create token
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.USERNAME.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }



    }
}