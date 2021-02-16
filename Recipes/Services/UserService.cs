using Recipes.Interfaces;
using Recipes.Models;
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
        {
        }
        public async Task<bool> CheckIfUserExists(string email) =>
            await _context.ApplicationUsers.AnyAsync(p => p.Email== email);

        public async Task<ApplicationUserDTO> CheckIfUserExists(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return null;
            return _mapper.Map<ApplicationUserDTO>(user);
        }
    }
}
