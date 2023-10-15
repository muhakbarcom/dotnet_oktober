global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_oktober.Services.UserService
{
    public class UserService : iUserService
    {
        private readonly DataContex _contex;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, DataContex contex)
        {
            _contex = contex;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> AddUser(AddUserDto newUser)
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();

            try
            {
                var User = _mapper.Map<User>(newUser);

                // Menambah karakter baru ke DbContext
                _contex.Users.Add(User);
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
                serviceResponse.Message = $"An error occurred while adding the User: {ex.Message}";
                
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> DeleteUsers(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();
            try
            {
                var dbUser = await _contex.Users.FirstOrDefaultAsync(c => c.ID == id);

                if (dbUser == null)
                    throw new Exception($"User with id '{id}' not found");


                _contex.Users.Remove(dbUser); // Hapus karakter dari DbContext
                await _contex.SaveChangesAsync(); // Simpan perubahan ke database

                // Mengambil daftar karakter yang tersisa dari database
                var remainingUsers = await _contex.Users.ToListAsync();
                serviceResponse.Data = remainingUsers.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
        {
            var servicesResponse = new ServiceResponse<List<GetUserDto>>();
            var dbUsers = await _contex.Users.ToListAsync();
            servicesResponse.Data = dbUsers.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            var servicesResponse = new ServiceResponse<GetUserDto>();

            var dbUser = await _contex.Users
                .Where(c => c.ID == id)
                .Select(c => _mapper.Map<GetUserDto>(c))
                .FirstOrDefaultAsync();

            servicesResponse.Data = dbUser;
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();
            try
            {
                // Cari karakter dalam database berdasarkan ID
                var User = await _contex.Users.FirstOrDefaultAsync(c => c.ID == updatedUser.ID);

                if (User == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User with id '{updatedUser.ID}' not found";
                    return serviceResponse;
                }

                // Update properti karakter dengan properti yang baru dari updatedUser
                _mapper.Map(updatedUser, User);

                // Simpan perubahan ke database
                _contex.Users.Update(User);
                await _contex.SaveChangesAsync();

                // Mengembalikan data karakter yang telah diperbarui
                serviceResponse.Data = _mapper.Map<GetUserDto>(User);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

    }
}