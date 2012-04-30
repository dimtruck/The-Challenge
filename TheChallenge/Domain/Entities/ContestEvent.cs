using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class ContestEvent
    {
        public string EventName { get; set; }
        public string EventType { get; set; }
        public string EventDescription { get; set; }
        public string EventGoal { get; set; }
    }
}
