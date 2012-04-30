using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class ContestEventGoalViewModel
    {
        public int Id { get; set; }
        public EventViewModel Event { get; set; }
        public ContestViewModel Contest { get; set; }
        public string Result { get; set; }

    }
}