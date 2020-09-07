using MealVault.Entities.Custom;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealVault.Services
{
    public static class SessionExtensions 
    {
        public static void SetAuthenticatedUser(this ISession session, string key, AuthenticatedUser authenticatedUser)
        {
            session.SetString(key, JsonConvert.SerializeObject(authenticatedUser));
        }

        public static T GetAuthenticatedUser<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
