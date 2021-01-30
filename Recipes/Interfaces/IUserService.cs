using Recipes.DTO;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUserDTO> CheckIfUserExists(string email);
    }
}
