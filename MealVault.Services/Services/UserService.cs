using MealVault.Entities;
using MealVault.Services.DataTransferModels;
using MealVault.Services.Interfaces;
using MealVault.Services.Repository;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MealVault.Services.Services
{
    public class UserService : PasswordService, IUserService
    {
        private readonly IGenericRepository<User> userRepository;
        private readonly IGenericRepository<FavoriteMeal> favoriteMealRepository;
        private readonly MealService mealService;


        public UserService(MealvaultContext context, MealService mealService)
        {
            userRepository = new GenericRepository<User>(context);
            favoriteMealRepository = new GenericRepository<FavoriteMeal>(context);
            this.mealService = mealService;
        }

        public async Task<bool> ValidateIfUsernameExist(string username, CancellationToken token = default)
        {
            try
            {
                User doesUserExist = await userRepository.FirstByConditionAsync(x => x.Username == username, token);

                if (doesUserExist != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public async Task SaveUser(User user, CancellationToken token = default)
        {
            try
            {
                HashSalt hashSalt = GenerateSaltedHash(32, user.Password);

                user.Password = hashSalt.Hash;
                user.Salt = hashSalt.Salt;


                await userRepository.AddAndSaveAsync(user, token);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public async Task<bool> ValidateIfPasswordsMatch(string username, string password, CancellationToken token = default)
        {
            try
            {
                User user = await userRepository.FirstByConditionAsync(x => x.Username == username, token);

                bool arePasswordsTheSame = VerifyPassword(password, user.Password, user.Salt);

                return arePasswordsTheSame;
            }
            catch (Exception error)
            {
                throw error;
            }
        }


        public async Task AddFavoriteMeal(FavoriteMeal favoriteMeal, CancellationToken token = default)
        {
            await favoriteMealRepository.AddAndSaveAsync(favoriteMeal, token);
        }

        public async Task RemoveFavoriteMeal(FavoriteMeal favoriteMeal, CancellationToken token = default)
        {
            await favoriteMealRepository.RemoveAndSaveAsync(favoriteMeal, token);
        }

        public async Task<bool> VerifyIfMealIsInFavorites(int id, string username, CancellationToken token = default)
        {
            FavoriteMeal favoriteMeal = await favoriteMealRepository.FirstByConditionAsync(x => x.MealID == id && x.Username == username);

            if (favoriteMeal != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<MealDTO>> GetFavoriteMealsByUsername(string username, CancellationToken token = default)
        {
            List<MealDTO> mealDTOList = new List<MealDTO>();
            List<FavoriteMeal> favoriteMealsList = (await favoriteMealRepository.FindAllAsync(x => x.Username == username)).ToList();

            foreach (FavoriteMeal favoriteMeal in favoriteMealsList)
            {
                var meal = await mealService.GetMealByID(favoriteMeal.MealID, token);

                mealDTOList.Add(meal);
            }

            return mealDTOList;
        }

        public async Task<User> GetUserByUsername(string username, CancellationToken token = default)
        {
            return await userRepository.FirstByConditionAsync(x => x.Username == username, token);
        }

        public async Task<User> GetUserByID(int id, CancellationToken token = default)
        {
            return await userRepository.FirstByConditionAsync(x => x.ID == id, token);
        }

        public async Task<FavoriteMeal> GetFavoriteMealByIDAndUsername(int mealID, string username, CancellationToken token = default)
        {
            var test = await favoriteMealRepository.FirstByConditionAsync(x => x.MealID == mealID && x.Username == username, token);

            return test;
        } 
    }
}
