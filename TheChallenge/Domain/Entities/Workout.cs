using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Workout
    {
        public IList<ExerciseEntry> ExerciseEntries { get; set; }
        public DateTime WorkoutDate { get; set; }
    }
}
