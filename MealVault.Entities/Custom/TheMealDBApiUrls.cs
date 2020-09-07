using System;
using System.Collections.Generic;
using System.Text;

namespace MealVault.Entities.Custom
{
    public class TheMealDBApiUrls
    { 
        public string SearchByName { get; set; }
        public string ListAllByFirstLetter {get; set;}
        public string SearchByID {get; set;}
        public string GetRandomMeal {get; set;}
        public string GetAllCategoriesWithDescription {get; set;}
        public string GetAllCategories {get; set;}
        public string GetAllAreas {get; set;}
        public string GetAllIngredients {get; set;}
        public string GetByMainIngredient {get; set;}
        public string GetByCategory {get; set;}
        public string GetByArea {get; set;}
        public string GetMealByArea { get; set;}
        public string GetMealByCategory { get; set;}
        public string GetMealByIngredient { get; set;}
    }
}
