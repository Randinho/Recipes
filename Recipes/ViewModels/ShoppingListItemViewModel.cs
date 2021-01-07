using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.ViewModels
{
    public class ShoppingListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool InPossession { get; set; }
    }
}
