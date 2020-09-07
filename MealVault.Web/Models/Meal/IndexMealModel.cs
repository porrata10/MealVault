using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealVault.Web.Models.Meal
{
    public class IndexMealModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Tags { get; set; }
        public string Picture { get; set; }
        public bool IsFavorite { get; set; }
    }
}
