using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis.Generic;
using Domain.Entities;
using ServiceStack.Redis;

namespace Domain.Cache
{
    public class ContestClient
    {
        private static readonly String selectContests = "urn:contests";
        private static readonly String selectContestEvents = @"urn:contests:{0}";

        #region "Contests"
        public static IList<Contest> RetrieveContests()
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<Contest>>(selectContests);
                }
            }
        }

        public static bool SaveContests(IList<Contest> contests)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<Contest>>(selectContests, contests);
                }
            }
        }

        public static bool RemoveContest(Contest contest)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    var contests = client.Get<IList<Contest>>(selectContests);
                    if (contests != null)
                    {
                        Contest contestToRemove = contests.FirstOrDefault(t => t.ContestId == contest.ContestId);
                        contests.Remove(contestToRemove);
                    }
                    return client.Set<IList<Contest>>(selectContests, contests);
                }
            }
        }

        public static bool AddContest(Contest contest)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    var contests = client.Get<IList<Contest>>(selectContests);
                    if (contests != null)
                        contests.Add(contest);
                    return client.Set<IList<Contest>>(selectContests, contests);
                }
            }
        }

        #endregion

        #region "ContestEvents"
        public static IList<ContestEvent> RetrieveContestEvents(int contestId)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<ContestEvent>>(String.Format(selectContestEvents, contestId));
                }
            }
        }

        public static bool SaveContestEvents(IList<ContestEvent> contestEvents, int contestId)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<ContestEvent>>(String.Format(selectContestEvents,contestId), contestEvents);
                }
            }
        }

        public static bool RemoveContestEvent(ContestEvent contestEvent, int contestId)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    var contestEvents = client.Get<IList<ContestEvent>>(String.Format(selectContestEvents, contestId));
                    if (contestEvents != null)
                    {
                        ContestEvent contestEventToRemove = contestEvents.FirstOrDefault(t => t.EventName.Equals(contestEvent.EventName));
                        contestEvents.Remove(contestEventToRemove);
                    }
                    return client.Set<IList<ContestEvent>>(String.Format(selectContestEvents, contestId), contestEvents);
                }
            }
        }

        public static bool AddContestEvent(ContestEvent contestEvent, int contestId)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    var contestEvents = client.Get<IList<ContestEvent>>(String.Format(selectContestEvents, contestId));
                    if (contestEvents != null)
                        contestEvents.Add(contestEvent);
                    return client.Set<IList<ContestEvent>>(String.Format(selectContestEvents, contestId), contestEvents);
                }
            }
        }

        #endregion
    }
}
