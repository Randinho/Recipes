using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipes.Data;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Interfaces.Repositories;
using Recipes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class ShareService : IShareService
    {
        private readonly IMapper _mapper;
        private readonly INotificationSender _notificationSender;
        private readonly IShareRepository _shareRepository;

        public ShareService(IMapper mapper,
            INotificationSender notificationSender,
            IShareRepository shareRepository)
        {
            _mapper = mapper;
            _notificationSender = notificationSender;
            _shareRepository = shareRepository;
        }
        public async Task<IEnumerable<SharedDTO>> GetSharedRecipesList(string userId)
        {
            var shared = await _shareRepository.GetSharedRecipesList(userId);
            var mapped = _mapper.Map<SharedDTO[]>(shared);
            return mapped;
        }
        public async Task<bool> ShareRecipe(int recipeId, ApplicationUserDTO user)
        {
            if (!await _shareRepository.IsAlreadyShared(recipeId, user.Id))
            {
                await _shareRepository.AddShared(user.Id, recipeId);
                _notificationSender.SendNotification("You received a recipe. Check it out in 'shared with me' tab.", user.Id);
                return true;
            }
            return false;

        }
    }
}
