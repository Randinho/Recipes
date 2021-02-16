﻿using Recipes.Interfaces;
using Recipes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Data;
using Microsoft.EntityFrameworkCore;
using Recipes.DTO;
using AutoMapper;

namespace Recipes.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryFilterViewModel>> GetCategoryFilters(IEnumerable<int> checkedFilters)
        {
            var categories = await _context.Categories.ToListAsync();
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
        public async Task<IEnumerable<CategoryDTO>> GetCategoryList()
        {
            var categories = await _context.Categories.ToListAsync();
            var mapped = _mapper.Map<CategoryDTO[]>(categories);
            return mapped;
        }
    }
}