using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Password { get; set; }

        public String Role { get; set; }

        public bool IsValid { get; set; }
    }
}
