using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager; 

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.context = context;
            this.userManager = userManager;
        }

        private string GetCurrentUserId()
        {
            ClaimsPrincipal current = this.User;
            return userManager.GetUserId(current);
        }
        

        public IActionResult Index()
        {
            var isAnyNotReceivedNotification = context.Notifications.FirstOrDefault(x => x.ReceiverId == GetCurrentUserId() && x.IsReceived == false);
            if (isAnyNotReceivedNotification != null)
                return View(true);
            else
                return View(false);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/permission")]
        public IActionResult PermissionDenied(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
