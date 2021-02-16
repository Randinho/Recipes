using Recipes.Interfaces;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckIfUserExists(string email) =>
            await _context.ApplicationUsers.AnyAsync(p => p.Email== email);
        
    }
}
