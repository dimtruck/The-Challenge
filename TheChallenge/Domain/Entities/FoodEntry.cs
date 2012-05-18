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
        public int FoodId { get; set; }
        public float ServingSize { get; set; }
        public short ServingId { get; set; }
        public string ServingDesc { get; set; }
    }
}
