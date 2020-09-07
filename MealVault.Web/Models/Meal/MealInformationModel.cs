using MealVault.Services.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealVault.Web.Models
{
    public class MealInformationModel
    {
        public MealInformationModel()
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
        public string Video { get; set; }
        public IEnumerable<string> IngredientsList { get; set; }
        public IEnumerable<string> MeasuresList { get; set; }
        public string Source { get; set; }

    }
}
