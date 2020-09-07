using MealVault.Services.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MealVault.Services.DataTransferModels
{
    public class MealDTO
    {
        public MealDTO()
        {
            InstructionsList = new List<string>();
            IngredientsList = new List<string>();
            MeasuresList = new List<string>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public IEnumerable<string> InstructionsList { get; set; }
        public string Picture { get; set; }
        public string Tags { get; set; }
        public string[] Video { get; set; }
        public bool IsFavorite { get; set; }
        public IEnumerable<string> IngredientsList { get; set; }
        public IEnumerable<string> MeasuresList { get; set; }
        public string Source { get; set; }

        internal void ConvertJsonResultToModelInformation(JsonMealResult jsonResult)
        {
            ID = Convert.ToInt32(jsonResult.meals[0].idMeal);
            Name = jsonResult.meals[0].strMeal;
            Category = jsonResult.meals[0].strCategory;
            Area = jsonResult.meals[0].strArea;
            Tags = jsonResult.meals[0].strTags;
            Picture = jsonResult.meals[0].strMealThumb;
            Video = jsonResult.meals[0].strYoutube != null ? jsonResult.meals[0].strYoutube.Split("=") : new string[0];
            InstructionsList = jsonResult.meals[0].strInstructions != null ? jsonResult.meals[0].strInstructions.Split(".").ToList() : new List<string>();
            IngredientsList = jsonResult != null ? CreateIngredientsList(jsonResult) : new List<string>();
            MeasuresList = jsonResult != null ? CreateMeasuresList(jsonResult) : new List<string>();
        }


        internal void ConvertJsonResultToModel(JsonMealResult jsonResult)
        {
            ID = Convert.ToInt32(jsonResult.meals[0].idMeal);
            Name = jsonResult.meals[0].strMeal;
            Picture = jsonResult.meals[0].strMealThumb;
        }

        internal void ConvertMealtoMealDTO(Meal meal)
        {
            ID = Convert.ToInt32(meal.idMeal);
            Name = meal.strMeal;
            Picture = meal.strMealThumb;
        }

        internal List<string> CreateIngredientsList(JsonMealResult jsonResult)
        {
            List<string> instructionsList = new List<string>();

            instructionsList.Add(jsonResult.meals[0].strIngredient1);
            instructionsList.Add(jsonResult.meals[0].strIngredient2);
            instructionsList.Add(jsonResult.meals[0].strIngredient3);
            instructionsList.Add(jsonResult.meals[0].strIngredient4);
            instructionsList.Add(jsonResult.meals[0].strIngredient5);
            instructionsList.Add(jsonResult.meals[0].strIngredient6);
            instructionsList.Add(jsonResult.meals[0].strIngredient7);
            instructionsList.Add(jsonResult.meals[0].strIngredient8);
            instructionsList.Add(jsonResult.meals[0].strIngredient9);
            instructionsList.Add(jsonResult.meals[0].strIngredient10);
            instructionsList.Add(jsonResult.meals[0].strIngredient11);
            instructionsList.Add(jsonResult.meals[0].strIngredient12);
            instructionsList.Add(jsonResult.meals[0].strIngredient13);
            instructionsList.Add(jsonResult.meals[0].strIngredient14);
            instructionsList.Add(jsonResult.meals[0].strIngredient15);
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strIngredient16));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strIngredient17));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strIngredient18));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strIngredient19));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strIngredient20));

            // to eliminate null, empty or strings that have whitespace from the list
            return instructionsList.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
        }


        internal List<string> CreateIngredientsList(Meal meal)
        {
            List<string> instructionsList = new List<string>();

            instructionsList.Add(meal.strIngredient1);
            instructionsList.Add(meal.strIngredient2);
            instructionsList.Add(meal.strIngredient3);
            instructionsList.Add(meal.strIngredient4);
            instructionsList.Add(meal.strIngredient5);
            instructionsList.Add(meal.strIngredient6);
            instructionsList.Add(meal.strIngredient7);
            instructionsList.Add(meal.strIngredient8);
            instructionsList.Add(meal.strIngredient9);
            instructionsList.Add(meal.strIngredient10);
            instructionsList.Add(meal.strIngredient11);
            instructionsList.Add(meal.strIngredient12);
            instructionsList.Add(meal.strIngredient13);
            instructionsList.Add(meal.strIngredient14);
            instructionsList.Add(meal.strIngredient15);
            instructionsList.Add(Convert.ToString(meal.strIngredient16));
            instructionsList.Add(Convert.ToString(meal.strIngredient17));
            instructionsList.Add(Convert.ToString(meal.strIngredient18));
            instructionsList.Add(Convert.ToString(meal.strIngredient19));
            instructionsList.Add(Convert.ToString(meal.strIngredient20));

            // to eliminate null, empty or strings that have whitespace from the list
            return instructionsList.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
        }


        internal List<string> CreateMeasuresList(JsonMealResult jsonResult)
        {
            List<string> instructionsList = new List<string>();

            instructionsList.Add(jsonResult.meals[0].strMeasure1);
            instructionsList.Add(jsonResult.meals[0].strMeasure2);
            instructionsList.Add(jsonResult.meals[0].strMeasure3);
            instructionsList.Add(jsonResult.meals[0].strMeasure4);
            instructionsList.Add(jsonResult.meals[0].strMeasure5);
            instructionsList.Add(jsonResult.meals[0].strMeasure6);
            instructionsList.Add(jsonResult.meals[0].strMeasure7);
            instructionsList.Add(jsonResult.meals[0].strMeasure8);
            instructionsList.Add(jsonResult.meals[0].strMeasure9);
            instructionsList.Add(jsonResult.meals[0].strMeasure10);
            instructionsList.Add(jsonResult.meals[0].strMeasure11);
            instructionsList.Add(jsonResult.meals[0].strMeasure12);
            instructionsList.Add(jsonResult.meals[0].strMeasure13);
            instructionsList.Add(jsonResult.meals[0].strMeasure14);
            instructionsList.Add(jsonResult.meals[0].strMeasure15);
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strMeasure16));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strMeasure17));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strMeasure18));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strMeasure19));
            instructionsList.Add(Convert.ToString(jsonResult.meals[0].strMeasure20));

            // to eliminate null, empty or strings that have whitespace from the list
            return instructionsList.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
        }


        internal List<string> CreateMeasuresList(Meal meal)
        {
            List<string> instructionsList = new List<string>();

            instructionsList.Add(meal.strMeasure1);
            instructionsList.Add(meal.strMeasure2);
            instructionsList.Add(meal.strMeasure3);
            instructionsList.Add(meal.strMeasure4);
            instructionsList.Add(meal.strMeasure5);
            instructionsList.Add(meal.strMeasure6);
            instructionsList.Add(meal.strMeasure7);
            instructionsList.Add(meal.strMeasure8);
            instructionsList.Add(meal.strMeasure9);
            instructionsList.Add(meal.strMeasure10);
            instructionsList.Add(meal.strMeasure11);
            instructionsList.Add(meal.strMeasure12);
            instructionsList.Add(meal.strMeasure13);
            instructionsList.Add(meal.strMeasure14);
            instructionsList.Add(meal.strMeasure15);
            instructionsList.Add(Convert.ToString(meal.strMeasure16));
            instructionsList.Add(Convert.ToString(meal.strMeasure17));
            instructionsList.Add(Convert.ToString(meal.strMeasure18));
            instructionsList.Add(Convert.ToString(meal.strMeasure19));
            instructionsList.Add(Convert.ToString(meal.strMeasure20));

            // to eliminate null, empty or strings that have whitespace from the list
            return instructionsList.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
        }
    }
}
