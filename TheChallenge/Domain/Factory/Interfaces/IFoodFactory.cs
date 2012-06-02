using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Repository;
using Domain.Entities;

namespace Domain.Factory.Interfaces
{
    public interface IFoodFactory
    {
        IList<Food> RetrieveFoods(IFoodRepository repository);
        Food RetrieveCompleteFood(int id, IFoodRepository repository);
    }
}
