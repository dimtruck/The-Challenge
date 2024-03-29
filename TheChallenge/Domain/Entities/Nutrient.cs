﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Nutrient
    {
        public int Id { get; set; }
        public String Units { get; set; }
        public String Description { get; set; }
        public decimal AmountIn100Grams { get; set; }
        public String SourceCode { get; set; }
        public String DerivCode { get; set; }
        public bool IsNutrientAdded { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
