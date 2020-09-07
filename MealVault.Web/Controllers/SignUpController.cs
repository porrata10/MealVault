using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MealVault.Entities.Custom;
using MealVault.Services;
using MealVault.Services.Interfaces;
using MealVault.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MealVault.Web.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IUserService userService;

        public SignUpController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignUpViewModel model, CancellationToken token = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                bool doesUsernameAlreadyExist = await userService.ValidateIfUsernameExist(model.Username, token);

                if (doesUsernameAlreadyExist == true)
                {
                    ModelState.AddModelError("Username", "Username already exist, provide another one");

                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords does not match");

                    return View(model);
                }

                Entities.User user = new Entities.User()
                {
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Username = model.Username,
                    MobileNumber = model.MobileNumber,
                    Password = model.Password
                };

                await userService.SaveUser(user);


                AuthenticatedUser userInSession = new AuthenticatedUser()
                {
                    Username = model.Username,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    MobileNumber = model.MobileNumber
                };


                HttpContext.Session.SetAuthenticatedUser("AuthenticatedUser", userInSession);


                return RedirectToAction("Index", "Meal");
            }
            catch (Exception error)
            {
                throw error;
            }
        }
    }
}
