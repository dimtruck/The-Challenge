using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class ServingViewModel
    {
        public int FoodId { get; set; }
        public int Id { get; set; }
        public float Amount { get; set; }
        public String Description { get; set; }
        public float WeightInGrams { get; set; }
    }
}