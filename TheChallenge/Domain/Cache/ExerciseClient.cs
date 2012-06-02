using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using ServiceStack.Redis;

namespace Domain.Cache
{
    public class ExerciseClient
    {
        private static readonly String selectExercises = @"urn:exercises";

        public static IList<Event> RetrieveExercises()
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<Event>>(selectExercises);
                }
            }
        }

        public static bool SaveExercises(IList<Event> exercises)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<Event>>(selectExercises, exercises);
                }
            }
        }


    }
}
