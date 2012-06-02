using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Domain.Entities;

namespace Domain.Cache
{
    public class WorkoutClient
    {
        private static readonly String selectWorkoutDates = @"urn:meal_workout_dates";
        private static readonly String selectWorkout = @"urn:workout:{0}";

        #region "Workout"
        public static IList<DateTime> RetrieveWorkoutDates()
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<IList<DateTime>>(selectWorkoutDates);
                }
            }
        }

        public static bool SaveWorkoutDates(IList<DateTime> workoutDates)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Add<IList<DateTime>>(selectWorkoutDates, workoutDates);
                }
            }
        }

        public static bool AddWorkoutDate(DateTime workoutDate){
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    IList<DateTime> workoutDates = client.Get<IList<DateTime>>(selectWorkoutDates);
                    if (!workoutDates.Contains(workoutDate))
                        workoutDates.Add(workoutDate);
                    return client.Set<IList<DateTime>>(selectWorkoutDates, workoutDates);
                }
            }

        }

        public static Workout RetrieveWorkout(DateTime dateTime)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    return client.Get<Workout>(String.Format(selectWorkout,dateTime));
                }
            }
        }

        public static bool SaveWorkout(Workout workout, DateTime dateTime)
        {
            using (var redisManager = new PooledRedisClientManager("91081b784147c027293e7d3acb816e94@lab.redistogo.com:9038"))
            {
                using (var client = redisManager.GetCacheClient())
                {
                    //check if workout for that date already exists
                    Workout existingWorkout = client.Get<Workout>(String.Format(selectWorkout, dateTime));
                    if (existingWorkout != null && existingWorkout.ExerciseEntries != null)
                    {
                        foreach (var exerciseEntry in workout.ExerciseEntries)
                        {
                            existingWorkout.ExerciseEntries.Add(exerciseEntry);
                        }
                        return client.Set<Workout>(String.Format(selectWorkout, dateTime), existingWorkout);
                    }
                    else
                    {
                        AddWorkoutDate(dateTime);
                        return client.Add<Workout>(String.Format(selectWorkout, dateTime), workout);
                    }
                }
            }
        }
        #endregion

    }
}
