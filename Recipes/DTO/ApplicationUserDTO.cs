using Microsoft.AspNetCore.Identity;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.DTO
{
    public class ApplicationUserDTO : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<RecipeDTO> Recipes { get; set; }
        public ICollection<FavoriteDTO> Favorites { get; set; }
        public ICollection<SharedDTO> Shared { get; set; }
        public ICollection<NotificationDTO> Notifications { get; set; }

        public ApplicationUserDTO()
        {
            Recipes = new Collection<RecipeDTO>();
            Favorites = new Collection<FavoriteDTO>();
            Shared = new Collection<SharedDTO>();
            Notifications = new Collection<NotificationDTO>();
        }
    }
}
