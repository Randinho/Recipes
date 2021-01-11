using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Recipes.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Shared> Shared { get; set; }
        public ICollection<Notification> Notifications { get; set; }

        public ApplicationUser()
        {
            Recipes = new Collection<Recipe>();
            Favorites = new Collection<Favorite>();
            Shared = new Collection<Shared>();
            Notifications = new Collection<Notification>();
        }
    }
}
