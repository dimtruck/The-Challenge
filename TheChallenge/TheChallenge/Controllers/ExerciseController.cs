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
    public class ExerciseController : ApiController
    {
        private readonly IExerciseRepository repository;
        private readonly IWorkoutRepository workoutRepository;
        private readonly IExerciseFactory exerciseFactory;
        private readonly IWorkoutFactory workoutFactory;

        public ExerciseController(IExerciseFactory factory, IWorkoutFactory workoutFactory, IExerciseRepository repository, IWorkoutRepository workoutRepository)
        {
            this.repository = repository;
            this.workoutRepository = workoutRepository;
            this.exerciseFactory = factory;
            this.workoutFactory = workoutFactory;
        }

        // GET /api/exercise
        [CustomAuthorize]
        public IEnumerable<EventViewModel> Get()
        {
            IList<Event> eventList = this.exerciseFactory.RetrieveExercises(repository);
            IList<EventViewModel> eventViewModelList = new List<EventViewModel>();
            if (eventList != null)
                foreach (Event eventModel in eventList)
                    eventViewModelList.Add(AutoMapper.Mapper.Map<EventViewModel>(eventModel));

            return eventViewModelList;
        }

        // POST /api/exercise
        [CustomAuthorize]
        public HttpResponseMessage Post(SaveWorkoutViewModel value)
        {
            //TODO: move to validation logic
            if (value == null)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "no valid request content present" };
            if(value.EntryDate == null || value.EntryDate == DateTime.MinValue || value.EntryDate == DateTime.MaxValue)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "no valid date present" };
            if (value.Exercises == null || value.Exercises.Count == 0)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "no exercises present" };
            Workout workout = new Workout()
            {
                ExerciseEntries = new List<ExerciseEntry>(),
                WorkoutDate = value.EntryDate.Date
            };
            foreach (SaveExerciseViewModel saveExerciseViewModel in value.Exercises)
            {
                workout.ExerciseEntries.Add(AutoMapper.Mapper.Map<ExerciseEntry>(saveExerciseViewModel));
            }
            if (workoutFactory.SaveWorkout(workout, workoutRepository))
                return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
            else
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "unable to save workout" };
        }
    }
}
