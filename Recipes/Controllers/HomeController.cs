using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Recipes.Interfaces;
using Recipes.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationService _notificationService;

        public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService) : base(userManager)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _notificationService.AnyNotReceivedNotification(GetCurrentUserId()));
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
