using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class CurrentStatistic
    {
        public String Exercise { get; set; }
        public DateTime LastExecuted { get; set; }
        public String Result { get; set; }
        public String EventGoalType { get; set; }
    }
}
