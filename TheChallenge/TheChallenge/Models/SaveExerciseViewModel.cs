using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class SaveExerciseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Distance { get; set; }
        public short? Reps { get; set; }
        public double? Weight { get; set; }
        public string Time { get; set; }
    }
}