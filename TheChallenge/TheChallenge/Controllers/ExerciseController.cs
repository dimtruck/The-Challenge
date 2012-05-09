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
    public class ExerciseController : ApiController
    {
        private readonly IExerciseRepository repository;
        private readonly IWorkoutRepository workoutRepository;

        public ExerciseController(IExerciseRepository repository, IWorkoutRepository workoutRepository)
        {
            this.repository = repository;
            this.workoutRepository = workoutRepository;
        }

        // GET /api/exercise
        [CustomAuthorize]
        public IEnumerable<EventViewModel> Get()
        {
            IList<Event> eventList = this.repository.RetrieveExercises();
            IList<EventViewModel> eventViewModelList = new List<EventViewModel>();
            foreach (Event eventModel in eventList)
                eventViewModelList.Add(AutoMapper.Mapper.Map<EventViewModel>(eventModel));

            return eventViewModelList;
        }

        // GET /api/exercise/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/exercise
        [CustomAuthorize]
        public HttpResponseMessage Post(SaveWorkoutViewModel value)
        {
            Workout workout = new Workout(){
                ExerciseEntries = new List<ExerciseEntry>(),
                WorkoutDate = value.EntryDate.Date
            };
            foreach (SaveExerciseViewModel saveExerciseViewModel in value.Exercises)
            {
                workout.ExerciseEntries.Add(AutoMapper.Mapper.Map<ExerciseEntry>(saveExerciseViewModel));
            }
            workoutRepository.SaveWorkout(workout);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }

        // PUT /api/exercise/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/exercise/5
        public void Delete(int id)
        {
        }
    }
}
