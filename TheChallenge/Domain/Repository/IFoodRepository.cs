using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IFoodRepository
    {
        IList<Food> RetrieveFoods();
        Food RetrieveFoodById(int foodId);
        IList<Nutrient> RetrieveAllNutrients();
        IList<Nutrient> RetrieveNutrientsForFoodId(int foodId);
        IList<Serving> RetrieveAllServings();
        IList<Serving> RetrieveServingsForFoodId(int foodId);
        Food RetrieveCompleteFood(int foodId);
    }
}
