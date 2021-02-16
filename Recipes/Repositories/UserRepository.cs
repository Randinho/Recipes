using Recipes.Interfaces.Repositories;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;
using Recipes.DTO;

namespace Recipes.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
