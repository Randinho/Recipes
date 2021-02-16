using Recipes.DTO;
using Recipes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryFilterViewModel>> GetCategoryFilters(IEnumerable<int> checkedFilters);
        Task<IEnumerable<CategoryDTO>> GetCategoriesList();
    }
}
