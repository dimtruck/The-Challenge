using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Contest
    {
        public int ContestId { get; set; }
        public String ContestName { get; set; }
        public DateTime ContestDate { get;set; }
        public String ContestDetails { get; set; }
        public String ContestPlace { get; set; }
    }
}
