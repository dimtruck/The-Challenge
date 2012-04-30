using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheChallenge.Models
{
    public class ContestViewModel
    {
        public int ContestId { get; set; }
        public String Name { get; set; }
        public DateTime ContestDate { get; set; }
        public String Place { get; set; }
        public String Details { get; set; }

    }
}