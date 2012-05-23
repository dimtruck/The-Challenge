using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class FoodEntry
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public String FoodId { get; set; }
        public double ServingSize { get; set; }
        public String ServingId { get; set; }
        public decimal TotalCalories { get; set; }
        public decimal TotalCarbs { get; set; }
        public decimal TotalFats { get; set; }
        public decimal TotalProtein { get; set; }

        public Serving SelectedServing { get; set; }
        public IList<Nutrient> CalculatedNutrients { get; set; }
    }
}
