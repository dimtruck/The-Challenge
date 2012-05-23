using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;
using TheChallenge.Helpers;

namespace TheChallenge.Controllers
{

    public class WorkoutController : ApiController
    {
        private readonly IWorkoutRepository repository;

        public WorkoutController(IWorkoutRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/workout
        [CustomAuthorize]
        public IEnumerable<DateTime> Get()
        {
            IList<DateTime> workoutDates = repository.GetWorkoutDates();
            return workoutDates;
        }

        // GET /api/workout/date
        [CustomAuthorize]
        public IList<SaveExerciseViewModel> Get(DateTime entryDate)
        {
            Workout workout = repository.GetWorkout(entryDate);
            IList<SaveExerciseViewModel> saveExerciseViewModelList = new List<SaveExerciseViewModel>();
            foreach (ExerciseEntry exerciseEntry in workout.ExerciseEntries)
            {
                saveExerciseViewModelList.Add(AutoMapper.Mapper.Map<SaveExerciseViewModel>(exerciseEntry));
            }
            return saveExerciseViewModelList;
        }
    }
}
