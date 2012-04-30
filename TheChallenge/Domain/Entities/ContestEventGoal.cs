using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class ContestEventGoal
    {
        public int Id { get; set; }
        public Event Event { get; set; }
        public Contest Contest { get; set; }
        public double? Weight { get; set; }
        public double? Distance { get; set; }
        public TimeSpan? Time { get; set; }
        public short? Reps { get; set; }
        public int GoalTypeId { get; set; }

    }
}
