using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class CurrentLiftsViewModel
    {
        public String Event { get; set; }

        public DateTime DateLifted { get; set; }

        public String Result { get; set; }

        public String EventType { get; set; }
    }
}