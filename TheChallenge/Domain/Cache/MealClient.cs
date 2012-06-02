using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using ServiceStack.Redis;

namespace Domain.Cache
{
    public class MealClient
    {
        private static readonly String saveMeals = @"urn:meals:{0}";
        private static readonly String getFoodEntries = "urn:foodEntries:{0}";

        public static IList<FoodEntry> GetFoodEntries(DateTime entryDate)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<FoodEntry>>(String.Format(getFoodEntries, entryDate));
                }
            }
        }

        public static bool SaveFoodEntries(DateTime entryDate, IList<FoodEntry> foodEntries)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    IList<FoodEntry> existingFoodEntries = client.Get<IList<FoodEntry>>(String.Format(getFoodEntries, entryDate));
                    if (existingFoodEntries != null)
                    {
                        foreach (var foodEntry in foodEntries)
                        {
                            existingFoodEntries.Add(foodEntry);
                        }
                        return client.Set<IList<FoodEntry>>(String.Format(getFoodEntries, entryDate), existingFoodEntries);
                    }
                    else
                        return client.Add<IList<FoodEntry>>(String.Format(getFoodEntries, entryDate), foodEntries);
                }
            }
        }

        public static bool SaveMeals(IList<Meal> meals)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    IList<bool> result = new List<bool>();
                    //save meals by date...add to workout dates
                    foreach (Meal meal in meals)
                    {
                        Meal tempMeal = client.Get<Meal>(String.Format(saveMeals, meal.MealDate));
                        if (tempMeal != null && tempMeal.Foods != null)
                        {
                            foreach (var food in meal.Foods)
                                tempMeal.Foods.Add(food);
                            result.Add(client.Set<Meal>(String.Format(saveMeals, meal.MealDate), tempMeal));
                        }
                        else
                        {
                            result.Add(client.Add<Meal>(String.Format(saveMeals, meal.MealDate), meal));
                            WorkoutClient.AddWorkoutDate(meal.MealDate.Date);
                        }
                    }

                    return !result.Contains(false);
                }
            }
        }
    }
}
