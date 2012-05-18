using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IMealRepository
    {
        bool SaveMeals(IList<Meal> meals);
        IList<Meal> GetMeals(DateTime entryDate);
    }
}
