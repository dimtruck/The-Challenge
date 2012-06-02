using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Factory.Interfaces;
using Domain.Repository;
using Domain.Cache;

namespace Domain.Factory
{
    public class MealFactory : IMealFactory
    {
        public bool SaveMeals(IList<Entities.Meal> meals, IMealRepository repository)
        {
            //save it in cache and in database
            //TODO: make async
            bool saveInCache = MealClient.SaveMeals(meals);
            bool saveInDB = repository.SaveMeals(meals);

            return saveInCache && saveInDB;
        }


        public IList<Entities.FoodEntry> GetFoodEntries(DateTime entryDate, IMealRepository repository)
        {
            //retrieve from cache
            IList<Entities.FoodEntry> foodEntries = MealClient.GetFoodEntries(entryDate);
            if (foodEntries == null)
            {
                //retrieve from database
                foodEntries = repository.GetFoodEntries(entryDate);
                //save to cache
                MealClient.SaveFoodEntries(entryDate, foodEntries);
            }

            return foodEntries;
        }
    }
}
