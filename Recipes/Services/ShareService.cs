using Recipes.DTO;
using Recipes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes.Services
{
    public class ShareService : IShareService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationSender _notificationSender;

        public ShareService(ApplicationDbContext context, IMapper mapper, INotificationSender notificationSender)
        {
            _context = context;
            _mapper = mapper;
            _notificationSender = notificationSender;
        }
        public async Task<IEnumerable<SharedDTO>> GetSharedRecipesList(string userId)
        {
            var shared = await _context.Shared
                .Include(s => s.Recipe.ApplicationUser)
                .Include(s => s.Recipe.Category)
                .Where(s => s.ApplicationUserId == userId).ToListAsync();
            var mapped = _mapper.Map<SharedDTO[]>(shared);
            return mapped;
        }
        public async Task<bool> ShareRecipe(int id, string email)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (!IsAlreadyShared(id, user.Id))
            {
                _context.Shared.Add(new Shared
                {
                    RecipeId = id,
                    ApplicationUserId = user.Id,
                    Confirmed = true
                });
                _notificationSender.SendNotification("You received a recipe. Check it out in 'shared with me' tab.", user.Id);
                await _context.SaveChangesAsync();
                return true;
            }
                return false;
            
        }

        private bool IsAlreadyShared(int recipeId, string userId) =>
            _context.Shared.Any(x => x.ApplicationUserId == userId && x.RecipeId == recipeId);
    }
}
