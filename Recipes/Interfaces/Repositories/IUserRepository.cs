using Recipes.DTO;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmail(string email);
    }
}
