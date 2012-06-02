using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Factory.Interfaces;
using Domain.Cache;
using Domain.Entities;
using Domain.Repository;

namespace Domain.Factory
{
    public class WorkoutFactory : IWorkoutFactory
    {
        public IList<DateTime> GetWorkoutDates(Repository.IWorkoutRepository repository)
        {
            //retrieve from cache
            IList<DateTime> dates = WorkoutClient.RetrieveWorkoutDates();
            if (dates == null)
            {
                //retrieve from database
                dates = repository.GetWorkoutDates();
                //save to cache
                WorkoutClient.SaveWorkoutDates(dates);
            }

            return dates;
        }

        public Entities.Workout GetWorkout(DateTime entryDate, Repository.IWorkoutRepository repository)
        {
            //retrieve from cache
            Workout workout = WorkoutClient.RetrieveWorkout(entryDate);
            if (workout == null)
            {
                //retrieve from database
                workout = repository.GetWorkout(entryDate);
                //save to cache
                WorkoutClient.SaveWorkout(workout, entryDate);
            }

            return workout;
        }


        public bool SaveWorkout(Workout workout, IWorkoutRepository repository)
        {
            //save it in cache and in database
            //TODO: make async
            bool saveInCache = WorkoutClient.SaveWorkout(workout, workout.WorkoutDate);
            bool saveInDB = repository.SaveWorkout(workout);

            return saveInCache && saveInDB;
        }
    }
}
