using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class ExerciseEntry
    {
        public int ExerciseId { get; set; }
        public String Name { get; set; }
        public short? Reps { get; set; }
        public double? Weight { get; set; }
        public TimeSpan? Time { get; set; }
        public double? Distance { get; set; }
    }
}
