using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using ServiceStack.Redis;

namespace Domain.Cache
{
    public class FoodClient
    {
        private static readonly String selectFoods = @"urn:foods";
        private static readonly String completeFood = @"urn:food:{0}";

        public static IList<Food> RetrieveFoods()
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<Food>>(selectFoods);
                }
            }
        }

        public static bool SaveFoods(IList<Food> foods)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<Food>>(selectFoods, foods);
                }
            }
        }


        public static Food RetrieveCompleteFood(int id)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<Food>(String.Format(completeFood, id.ToString()));
                }
            }
        }

        public static bool SaveCompleteFood(Food food, int id)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    //check if food for that id already exists
                    Food existingFood = client.Get<Food>(String.Format(completeFood, id.ToString()));
                    if (existingFood != null)
                    {
                        return client.Set<Food>(String.Format(completeFood, id.ToString()), existingFood);
                    }
                    else
                    {
                        return client.Add<Food>(String.Format(completeFood, id.ToString()), food);
                    }
                }
            }
        }


    }
}
