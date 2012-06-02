using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Factory.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Domain.Cache;

namespace Domain.Factory
{
    public class FoodFactory : IFoodFactory
    {
        public IList<Food> RetrieveFoods(IFoodRepository repository)
        {
            //retrieve from cache
            IList<Food> foods = FoodClient.RetrieveFoods();
            if (foods == null)
            {
                //retrieve from database
                foods = repository.RetrieveFoods();
                //save to cache
                FoodClient.SaveFoods(foods);
            }

            return foods;
        }

        public Food RetrieveCompleteFood(int id, IFoodRepository repository)
        {
            //retrieve from cache
            Food food = FoodClient.RetrieveCompleteFood(id);
            if (food == null)
            {
                //retrieve from database
                food = repository.RetrieveCompleteFood(id);
                //save to cache
                FoodClient.SaveCompleteFood(food, id);
            }

            return food;
        }
    }
}
