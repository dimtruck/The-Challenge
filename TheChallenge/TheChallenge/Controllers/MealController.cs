using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;

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
        public IList<MealEntryViewModel> Get(DateTime entryDate)
        {
            IList<FoodEntry> foodEntries = repository.GetFoodEntries(entryDate);
            IList<MealEntryViewModel> mealEntryViewModels = new List<MealEntryViewModel>();
            foreach (FoodEntry foodEntry in foodEntries)
                mealEntryViewModels.Add(AutoMapper.Mapper.Map<MealEntryViewModel>(foodEntry));
            
            return mealEntryViewModels;
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
