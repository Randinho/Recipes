using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.AspNetCore.Identity;
using Recipes.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Recipes.DTO;
using Recipes.Interfaces;

namespace Recipes.Controllers
{
    public class NotificationController : BaseController
    {     
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        public NotificationController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            INotificationService notificationService) : base(userManager)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await _notificationService.GetNotificationList(GetCurrentUserId());

            return View(notifications);
        }

        public async Task<IActionResult> SetNotificationReceived(int? id)
        {
            await _notificationService.SetNotificationsReceived(id, GetCurrentUserId());

            return RedirectToAction(nameof(Index));
        }
    }
}
