using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;
using Domain.Factory.Interfaces;

namespace TheChallenge.Controllers
{
    public class MealController : ApiController
    {
        private readonly IMealRepository repository;
        private readonly IMealFactory mealFactory;

        public MealController(IMealFactory factory, IMealRepository repository)
        {
            this.mealFactory = factory;
            this.repository = repository;
        }

        // GET /api/meal/5
        public IList<MealEntryViewModel> Get(DateTime entryDate)
        {
            IList<FoodEntry> foodEntries = mealFactory.GetFoodEntries(entryDate, repository);
            IList<MealEntryViewModel> mealEntryViewModels = new List<MealEntryViewModel>();
            if (foodEntries != null)
                foreach (FoodEntry foodEntry in foodEntries)
                    mealEntryViewModels.Add(AutoMapper.Mapper.Map<MealEntryViewModel>(foodEntry));
            
            return mealEntryViewModels;
        }
    }
}
