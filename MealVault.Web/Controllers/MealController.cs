using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MealVault.Entities;
using MealVault.Entities.Custom;
using MealVault.Services;
using MealVault.Services.DataTransferModels;
using MealVault.Services.Interfaces;
using MealVault.Services.JsonModels;
using MealVault.Services.Services;
using MealVault.Web.Models.Meal;
using MealVault.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;

namespace MealVault.Web.Controllers
{
    public class MealController : Controller
    {
        private readonly MealService mealService;
        private readonly IUserService userService;

        public MealController(MealService mealService, IUserService userService)
        {
            this.mealService = mealService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            MealViewModel mealViewModel = new MealViewModel();

            List<MealDTO> randomMeals = await mealService.GetRandomMeals();

            foreach (MealDTO randomMeal in randomMeals)
            {
                IndexMealModel meal = new IndexMealModel();

                AuthenticatedUser authUser = HttpContext.Session.GetAuthenticatedUser<AuthenticatedUser>("AuthenticatedUser");

                // verify if meal is favorite for the user in session
                FavoriteMeal favoriteMeal = await userService.GetFavoriteMealByIDAndUsername(randomMeal.ID, authUser.Username);

                meal.ID = randomMeal.ID;
                meal.Name = randomMeal.Name;
                //meal.Area = randomMeal.Area;
                //meal.Tags = randomMeal.Tags;
                meal.Picture = randomMeal.Picture;
                meal.IsFavorite = favoriteMeal == null ? false : true;

                mealViewModel.MealsList.Add(meal);
            }

            mealViewModel.AreasList = await mealService.GetAreasList();
            mealViewModel.CategoriesList = await mealService.GetCategoriesList();
            mealViewModel.IngredientsList = await mealService.GetIngredientsList();


            return View(mealViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(MealViewModel model, CancellationToken token = default)
        {
            model.AreasList = await mealService.GetAreasList();
            model.CategoriesList = await mealService.GetCategoriesList();
            model.IngredientsList = await mealService.GetIngredientsList();

            List<MealDTO> searchedMeals = await mealService.SearchForMeal(model.Area, model.Category, model.MainIngredient, token);

            foreach (MealDTO searchedMeal in searchedMeals)
            {
                IndexMealModel meal = new IndexMealModel();

                AuthenticatedUser authUser = HttpContext.Session.GetAuthenticatedUser<AuthenticatedUser>("AuthenticatedUser");

                // verify if meal is favorite for the user in session
                FavoriteMeal favoriteMeal = await userService.GetFavoriteMealByIDAndUsername(searchedMeal.ID, authUser.Username);

                meal.ID = searchedMeal.ID;
                meal.Name = searchedMeal.Name;
                meal.Picture = searchedMeal.Picture;
                meal.IsFavorite = favoriteMeal == null ? false : true;

                model.MealsList.Add(meal);
            }

            return View(model);
        }

            [HttpGet]
        public async Task<IActionResult> MealInformation(int id)
        {
            MealDTO mealInformation = await mealService.GetMealByID(id);

            MealInformationViewModel model = new MealInformationViewModel() 
            { 
                MealInformation = new Models.MealInformationModel() 
                {
                    ID = mealInformation.ID,
                    Name = mealInformation.Name,
                    Category = mealInformation.Category,
                    Area = mealInformation.Area,
                    InstructionsList = mealInformation.InstructionsList.SkipLast(1),
                    Picture = mealInformation.Picture,
                    Tags = mealInformation.Tags,
                    Video = mealInformation.Video[1],
                    IngredientsList = mealInformation.IngredientsList,
                    MeasuresList = mealInformation.MeasuresList,
                    Source = mealInformation.Source
                }
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddMealToFavorites(int id, bool isFavoriteCheckoxChecked, CancellationToken token = default)
        {
            try
            {
                AuthenticatedUser authUser = HttpContext.Session.GetAuthenticatedUser<AuthenticatedUser>("AuthenticatedUser");

                if (isFavoriteCheckoxChecked == true)
                {
                    Entities.FavoriteMeal favoriteMeal = new Entities.FavoriteMeal()
                    {
                        MealID = id,
                        Username = authUser.Username
                    };

                    await userService.AddFavoriteMeal(favoriteMeal, token);

                    return Ok(new { Message = "Meal was added to favorites" });
                }
                else
                {
                    Entities.FavoriteMeal favoriteMeal = await userService.GetFavoriteMealByIDAndUsername(id, authUser.Username, token);

                    await userService.RemoveFavoriteMeal(favoriteMeal, token);

                    return Ok(new { Message = "Meal was removed from favorites" });
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {

            MealViewModel mealViewModel = new MealViewModel();

            AuthenticatedUser authenticatedUser = HttpContext.Session.GetAuthenticatedUser<AuthenticatedUser>("AuthenticatedUser");

            List<MealDTO> randomMeals = await userService.GetFavoriteMealsByUsername(authenticatedUser.Username);

            foreach (MealDTO randomMeal in randomMeals)
            {
                IndexMealModel meal = new IndexMealModel();

                FavoriteMeal favoriteMeal = await userService.GetFavoriteMealByIDAndUsername(randomMeal.ID, HttpContext.Session.GetString("AuthenticatedUsername"));

                meal.ID = randomMeal.ID;
                meal.Name = randomMeal.Name;
                meal.Area = randomMeal.Area;
                meal.Tags = randomMeal.Tags;
                meal.Picture = randomMeal.Picture;
                meal.IsFavorite = favoriteMeal == null ? false : true;

                mealViewModel.MealsList.Add(meal);
            }

            return View(mealViewModel);
        }
    }
}
