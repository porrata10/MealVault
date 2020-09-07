using MealVault.Entities;
using MealVault.Services.DataTransferModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MealVault.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ValidateIfUsernameExist(string username, CancellationToken token = default);
        Task SaveUser(User user, CancellationToken token = default);
        Task<bool> ValidateIfPasswordsMatch(string username, string password, CancellationToken token = default);
        Task AddFavoriteMeal(FavoriteMeal favoriteMeal, CancellationToken token = default);
        Task<List<MealDTO>> GetFavoriteMealsByUsername(string username, CancellationToken token = default);
        Task<bool> VerifyIfMealIsInFavorites(int id, string username, CancellationToken token = default);
        Task<User> GetUserByUsername(string username, CancellationToken token = default);
        Task RemoveFavoriteMeal(FavoriteMeal favoriteMeal, CancellationToken token = default);
        Task<User> GetUserByID(int id, CancellationToken token = default);
        Task<FavoriteMeal> GetFavoriteMealByIDAndUsername(int mealID, string username, CancellationToken token = default);
    }
}
