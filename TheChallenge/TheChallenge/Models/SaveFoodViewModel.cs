using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class SaveFoodViewModel
    {
        public int FoodId { get; set; }
        public String Name { get; set; }
        public double ServingSize { get; set; }
        public int ServingTypeId { get; set; }
        public DateTime Date { get; set; }
    }
}