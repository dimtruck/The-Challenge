﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IFoodRepository
    {
        IList<Food> RetrieveFoodNames();
    }
}
