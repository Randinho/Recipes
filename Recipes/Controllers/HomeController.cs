﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Recipes.Data;
using Recipes.Models;
using System.Diagnostics;
using System.Linq;


namespace Recipes.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;    

        public HomeController(ILogger<HomeController> logger, 
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper) : base(userManager, mapper)
        {
            _logger = logger;
            _context = context;     
        }

        public IActionResult Index()
        {
            var isAnyNotReceivedNotification = _context.Notifications.FirstOrDefault(x => x.ReceiverId == GetCurrentUserId() && x.IsReceived == false);
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
