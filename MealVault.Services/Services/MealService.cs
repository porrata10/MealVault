using MealVault.Entities;
using MealVault.Entities.Custom;
using MealVault.Services.Interfaces;
using MealVault.Services.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using MealVault.Services.JsonModels;
using MealVault.Services.DataTransferModels;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace MealVault.Services.Services
{
    public class MealService
    {
        private readonly TheMealDBApiUrls mealDbUrls;
        private HttpClient client { get; }


        public MealService(MealvaultContext context, HttpClient client, IOptions<TheMealDBApiUrls> mealDbUrls)
        {

            client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
       
            this.client = client;
            this.mealDbUrls = mealDbUrls.Value;
        }

        /// <summary>
        /// Gets a meal by it's name from the TheMealDB API
        /// </summary>
        /// <param name="mealName"> name used for the search of the meal </param>
        /// <param name="token"> validation token </param>
        /// <returns>MealDTO</returns>
        public async Task<MealDTO> GetMealByName(string mealName, CancellationToken token = default)
        {
            try
            {
                JsonMealResult mealJSONResult = await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.SearchByName + mealName, token);

                MealDTO mealInformation = new MealDTO();

                mealInformation.ConvertJsonResultToModelInformation(mealJSONResult);

                return mealInformation;
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        /// <summary>
        /// Gets a meal by it's id from the TheMealDB API
        /// </summary>
        /// <param name="id"> id used for the search of the meal </param>
        /// <param name="token"> validation token </param>
        /// <returns>MealDTO</returns>
        public async Task<MealDTO> GetMealByID(int id, CancellationToken token = default)
        {
            try
            {
                JsonMealResult mealJSONResult = await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.SearchByID + id, token);

                MealDTO mealInformation = new MealDTO();

                mealInformation.ConvertJsonResultToModelInformation(mealJSONResult);

                return mealInformation;
            }
            catch (Exception error)
            {
                throw error;
            }
        }


        /// <summary>
        /// Gets 30 random meals from TheMealDB API
        /// </summary>
        /// <param name="token"> validation token </param>
        /// <returns>List<MealDTO></returns>
        public async Task<List<MealDTO>> GetRandomMeals(CancellationToken token = default)
        {
            try
            {
                List<MealDTO> mealResultList = new List<MealDTO>();

                for (int i = 0; i < 30; i++)
                {
                    JsonMealResult mealJSONResult = await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetRandomMeal, token);

                    MealDTO mealInformation = new MealDTO();

                    mealInformation.ConvertJsonResultToModel(mealJSONResult);

                    bool isMealInList = VerifyIfMealIsInList(mealResultList, mealInformation);

                    if (isMealInList != true)
                    {
                        mealResultList.Add(mealInformation);
                    }
                }


                // TODO: maybe add Distinct() to not return duplicate data
                return mealResultList;
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        /// <summary>
        /// Validates if a meal is in a List<MealDTO> object
        /// </summary>
        /// <param name="mealsList"> meals list </param>
        /// <param name="mealName"> name from the meal </param>
        /// <returns>bool</returns>
        public bool VerifyIfMealIsInList(List<MealDTO> mealsList, MealDTO mealName)
        {
            return mealsList.Contains(mealName);
        }

        public async Task<List<SelectListItem>> GetAreasList(CancellationToken token = default)
        {
            JsonMealResult mealJsonList = (await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetAllAreas, token));

            List<SelectListItem> areasList = mealJsonList.meals.Select(x => new SelectListItem()
            {
                Text = x.strArea,
                Value = x.strArea
            }).ToList() ?? new List<SelectListItem>();

            return areasList;
        }

        public async Task<List<SelectListItem>> GetCategoriesList(CancellationToken token = default)
        {
            JsonMealResult mealJsonList = (await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetAllCategories, token));

            List<SelectListItem> categoriesList = mealJsonList.meals.Select(x => new SelectListItem()
            {
                Text = x.strCategory,
                Value = x.strCategory
            }).ToList() ?? new List<SelectListItem>();

            return categoriesList;
        }

        public async Task<List<SelectListItem>> GetIngredientsList(CancellationToken token = default)
        {
            JsonMealResult mealJsonList = (await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetAllIngredients, token));

            List<SelectListItem> ingredientsList = mealJsonList.meals.Select(x => new SelectListItem()
            {
                Text = x.strIngredient,
                Value = x.strIngredient
            }).ToList() ?? new List<SelectListItem>();

            return ingredientsList;
        }


        public async Task<List<MealDTO>> SearchForMeal(string area, string category, string ingredient, CancellationToken token = default)
        {

            List<MealDTO> returnedList = new List<MealDTO>();
            List<Meal> searchedMealList = new List<Meal>();

            if (area != null)
            {
                JsonMealResult mealsByAreaList = (await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetMealByArea + area, token));
                searchedMealList.AddRange(mealsByAreaList.meals.ToList());
            }

            if (category != null)
            {
                JsonMealResult mealsByCategoryList = (await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetMealByCategory + category, token));
                searchedMealList.AddRange(mealsByCategoryList.meals.ToList());
            }

            if (ingredient != null)
            {
                JsonMealResult mealsByIngredientList = (await client.GetFromJsonAsync<JsonMealResult>(mealDbUrls.GetMealByIngredient + ingredient, token));
                searchedMealList.AddRange(mealsByIngredientList.meals.ToList());
            }

            foreach (Meal searchedMeal in searchedMealList)
            {
                MealDTO meal = new MealDTO();

                meal.ConvertMealtoMealDTO(searchedMeal);

                returnedList.Add(meal);
            }


            return returnedList.DistinctBy(x => x.Name).ToList();
        }
    }
}
