using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class SaveWorkoutViewModel
    {
        public IList<SaveExerciseViewModel> Exercises { get; set; }
        public DateTime EntryDate { get; set; }
    }
}