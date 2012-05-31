using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;
using TheChallenge.Helpers;
using Domain.Factory.Interfaces;

namespace TheChallenge.Controllers
{

    public class WorkoutController : ApiController
    {
        private readonly IWorkoutRepository repository;
        private readonly IWorkoutFactory factory;

        public WorkoutController(IWorkoutRepository repository, IWorkoutFactory factory)
        {
            this.repository = repository;
            this.factory = factory;
        }

        // GET /api/workout
        [CustomAuthorize]
        public IEnumerable<DateTime> Get()
        {
            IList<DateTime> workoutDates = factory.GetWorkoutDates(repository);
            return workoutDates;
        }

        // GET /api/workout/date
        [CustomAuthorize]
        public IList<SaveExerciseViewModel> Get(DateTime entryDate)
        {
            Workout workout = factory.GetWorkout(entryDate, repository);
            IList<SaveExerciseViewModel> saveExerciseViewModelList = new List<SaveExerciseViewModel>();
            if (workout != null && workout.ExerciseEntries != null)
                foreach (ExerciseEntry exerciseEntry in workout.ExerciseEntries)
                    saveExerciseViewModelList.Add(AutoMapper.Mapper.Map<SaveExerciseViewModel>(exerciseEntry));
            return saveExerciseViewModelList;
        }
    }
}
