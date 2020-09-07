using MealVault.Web.Models.Meal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealVault.Web.ViewModels
{
    public class MealViewModel
    {
        public MealViewModel()
        {
            MealsList = new List<IndexMealModel>();
            AreasList = new List<SelectListItem>();
            CategoriesList = new List<SelectListItem>();
            IngredientsList = new List<SelectListItem>();
        }


        public string Area { get; set; }
        public string Category { get; set; }
        public string MainIngredient { get; set; }
      public List<IndexMealModel> MealsList { get; set; }
      public List<SelectListItem> AreasList { get; set; }
      public List<SelectListItem> CategoriesList { get; set; }
      public List<SelectListItem> IngredientsList { get; set; }
    }
}
