using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Models;
using System.Security.Claims;

namespace Recipes.Controllers
{
    public class BaseController : Controller
    { 
       private UserManager<ApplicationUser> _userManager;
       
       public BaseController(UserManager<ApplicationUser> userManager)
       {
            _userManager = userManager;  
       }

        public string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            return _userManager.GetUserId(currentUser);
        }
    }
}
