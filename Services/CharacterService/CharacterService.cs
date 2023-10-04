global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_oktober.Services.CharacterService
{
    public class CharacterService : iCharacterService
    {
        private static List<Character> characters = new List<Character>{
            new Character(),
            new Character{Id=1, Name = "Sam"}
        };
        private readonly DataContex _contex;
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper, DataContex contex)
        {
            _contex = contex;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = _mapper.Map<Character>(newCharacter);

                // Menambah karakter baru ke DbContext
                _contex.Characters.Add(character);
                await _contex.SaveChangesAsync();

                // Mengambil daftar karakter yang sudah ada dari database
                var characters = await _contex.Characters.ToListAsync();
                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"An error occurred while adding the character: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacters(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var dbCharacter = await _contex.Characters.FirstOrDefaultAsync(c => c.Id == id);

                if (dbCharacter == null)
                    throw new Exception($"Character with id '{id}' not found");


                _contex.Characters.Remove(dbCharacter);
                await _contex.SaveChangesAsync();

                // Mengambil daftar karakter yang tersisa dari database
                var remainingCharacters = await _contex.Characters.ToListAsync();
                serviceResponse.Data = remainingCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var servicesResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _contex.Characters.ToListAsync();
            servicesResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var servicesResponse = new ServiceResponse<GetCharacterDto>();

            var dbCharacter = await _contex.Characters.FirstOrDefaultAsync(c => c.Id == id);
            servicesResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                // Cari karakter dalam database berdasarkan ID
                var character = await _contex.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Character with id '{updatedCharacter.Id}' not found";
                    return serviceResponse;
                }

                // Update properti karakter dengan properti yang baru dari updatedCharacter
                _mapper.Map(updatedCharacter, character);

                // Simpan perubahan ke database
                _contex.Characters.Update(character);
                await _contex.SaveChangesAsync();

                // Mengembalikan data karakter yang telah diperbarui
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
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