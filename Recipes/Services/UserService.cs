using Recipes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Recipes.Interfaces.Repositories;
using Recipes.Models;
using Recipes.DTO;
using AutoMapper;

namespace Recipes.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationUserDTO> CheckIfUserExists(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return null;
            return _mapper.Map<ApplicationUserDTO>(user);
        }
    }
}
