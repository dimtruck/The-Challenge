using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Domain.Entities;

namespace Domain.Cache
{
    public class ProfileClient
    {
        private static readonly String getGoals = @"urn:goals";
        private static readonly String getProfile = @"urn:profile";

        public static IList<Entities.ContestEventGoal> RetrieveGoals()
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<ContestEventGoal>>(getGoals);
                }
            }
        }

        public static bool SaveGoals(IList<ContestEventGoal> goals)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<ContestEventGoal>>(getGoals, goals);
                }
            }
        }

        public static IList<Entities.CurrentStatistic> RetrieveProfile()
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<CurrentStatistic>>(getProfile);
                }
            }
        }

        public static bool SaveProfile(IList<CurrentStatistic> profile)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<CurrentStatistic>>(getProfile, profile);
                }
            }
        }

    }
}
