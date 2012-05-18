using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.Repository;
using Domain.Entities;

namespace TheChallenge.Controllers
{
    public class MealController : ApiController
    {
        private readonly IMealRepository repository;

        public MealController(IMealRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/meal
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/meal/5
        public string Get(DateTime entryDate)
        {
            IList<Meal> meals = repository.GetMeals(entryDate);
            /*IList<SaveExerciseViewModel> saveExerciseViewModelList = new List<SaveExerciseViewModel>();
            foreach (ExerciseEntry exerciseEntry in workout.ExerciseEntries)
            {
                saveExerciseViewModelList.Add(AutoMapper.Mapper.Map<SaveExerciseViewModel>(exerciseEntry));
            }
            return saveExerciseViewModelList;*/
            return null;
        }

        // POST /api/meal
        public void Post(string value)
        {
        }

        // PUT /api/meal/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/meal/5
        public void Delete(int id)
        {
        }
    }
}
