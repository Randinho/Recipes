using AutoMapper;
using Recipes.DTO;
using Recipes.Interfaces;
using Recipes.Interfaces.Repositories;
using Recipes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesList()
        {
            var categories = await _categoryRepository.GetCategoryList();
            var mapped = _mapper.Map<CategoryDTO[]>(categories);
            return mapped;
        }

        public async Task<IEnumerable<CategoryFilterViewModel>> GetCategoryFilters(IEnumerable<int> checkedFilters)
        {
            var categories = await _categoryRepository.GetCategoryList();
            var filters = new List<CategoryFilterViewModel>();
            foreach (var item in categories)
            {
                var filter = new CategoryFilterViewModel
                {
                    Name = item.Name,
                    Id = item.Id
                };
                filter.IsChecked = checkedFilters.Contains(item.Id);
                filters.Add(filter);
            }
            return filters;
        }
    }
}
