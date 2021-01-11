using AutoMapper;
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
        
       private UserManager<ApplicationUser> _userManager;
        protected IMapper _mapper;
       
       public BaseController(UserManager<ApplicationUser> userManager, IMapper mapper)
       {
            _userManager = userManager;
            _mapper = mapper;
    
       }

        public string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            return _userManager.GetUserId(currentUser);
        }
    }
}
