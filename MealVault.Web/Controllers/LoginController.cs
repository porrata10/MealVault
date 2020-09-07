using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MealVault.Entities;
using MealVault.Entities.Custom;
using MealVault.Services;
using MealVault.Services.Interfaces;
using MealVault.Services.Services;
using MealVault.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MealVault.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly MealService mealService;


        public LoginController(IUserService userService, MealService mealService)
        {
            this.userService = userService;
            this.mealService = mealService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model, CancellationToken token = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                bool doesUsernameExist = await userService.ValidateIfUsernameExist(model.Username, token);

                if (doesUsernameExist == true)
                {
                    bool doesPasswordMatchUserAccount = await userService.ValidateIfPasswordsMatch(model.Username, model.Password, token);

                    if (doesPasswordMatchUserAccount == true)
                    {
                        User user = await userService.GetUserByUsername(model.Username, token);

                        AuthenticatedUser userInSession = new AuthenticatedUser()
                        {
                            Username = model.Username,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            MobileNumber = user.MobileNumber
                        };


                        HttpContext.Session.SetAuthenticatedUser("AuthenticatedUser", userInSession);


                        return RedirectToAction("Index", "Meal");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect password, please try another one.");

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "Username does not exist, please try another one.");

                    return View(model);
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }
    }
}
