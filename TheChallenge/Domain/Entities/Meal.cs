using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public DateTime MealDate { get; set; }
        public IList<FoodEntry> Foods { get; set; }
    }
}
