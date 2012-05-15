using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Serving
    {
        public int FoodId { get; set; }
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public String Description { get; set; }
        public decimal WeightInGrams { get; set; }
    }
}
