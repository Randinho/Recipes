using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class BaseController : Controller
    {
        
       public UserManager<ApplicationUser> userManager;
       
       public BaseController(UserManager<ApplicationUser> userManager)
       {
            this.userManager = userManager;
    
       }

        public string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            return userManager.GetUserId(currentUser);
        }
    }
}
