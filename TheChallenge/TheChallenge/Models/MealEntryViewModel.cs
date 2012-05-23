using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class MealEntryViewModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String EntryDate { get; set; }
        public String FoodId { get; set; }
        public double ServingSize { get; set; }
        public String ServingId { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal TotalCarbs { get; set; }
        public decimal TotalFats { get; set; }
        public decimal TotalProtein { get; set; }

        public ServingViewModel SelectedServing { get; set; }
        public IList<NutrientViewModel> CalculatedNutrients { get; set; }
    }
}