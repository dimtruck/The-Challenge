using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Food
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }
        public IList<Nutrient> AvailableNutrients { get; set; }
        public IList<Serving> AvailableServings { get; set; }
        public Serving SelectedServing { get; set; }
        public IList<Nutrient> CalculatedNutrients { get; set; }
    }
}
