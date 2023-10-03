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
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var servicesResponse = new ServiceResponse<List<GetCharacterDto>>();
            characters.Add(_mapper.Map<Character>(newCharacter));
            servicesResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return servicesResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var servicesResponse = new ServiceResponse<List<GetCharacterDto>>();
            servicesResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var servicesResponse = new ServiceResponse<GetCharacterDto>();

            var character = characters.FirstOrDefault(c => c.Id == id);
            servicesResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return servicesResponse;
        }
    }
}