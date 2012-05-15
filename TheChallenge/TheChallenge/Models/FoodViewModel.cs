using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class FoodViewModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public IList<NutrientViewModel> Nutrients { get; set; }
        public IList<ServingViewModel> Servings { get; set; }
    }
}