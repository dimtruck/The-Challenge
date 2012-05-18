﻿using System;
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
    public class FoodController : ApiController
    {
        private readonly IFoodRepository repository;
        private readonly IMealRepository mealRepository;

        public FoodController(IFoodRepository repository, IMealRepository mealRepository)
        {
            this.repository = repository;
            this.mealRepository = mealRepository;
        }

        // GET /api/food
        [CustomAuthorize]
        public IEnumerable<FoodViewModel> Get()
        {
            IList<Food> foodList = this.repository.RetrieveFoods();
            IList<FoodViewModel> foodModelList = new List<FoodViewModel>();
            foreach (Food food in foodList)
            {
                foodModelList.Add(AutoMapper.Mapper.Map<FoodViewModel>(food));
            }
            return foodModelList;
        }

        // GET /api/food/5
        public Food Get(int id)
        {
            //get entire food information
            //FOOD OBJECT
            //  food category (string)
            //  footnote (optional) (string)
            //  langual (string)
            //  nutrient data list (NUTRIENT OBJECT)
            //      units (string)
            //      description (string)
            //      amount_in_100 (float)
            //      source_code (string)
            //      deriv_code (string)
            //      is_nutrient_added (boolean)
            //      last_updated (datetime)
            //  weight list (FOOD_WEIGHT OBJECT)
            //      seq (int)
            //      amount (float)
            //      description (string)
            //      weight_in_grams (float)
            Food food = repository.RetrieveCompleteFood(id);
            if (food != null)
            {
                FoodViewModel model = new FoodViewModel()
                {
                    Nutrients = new List<NutrientViewModel>(),
                    Servings = new List<ServingViewModel>()
                };
                if(food.AvailableNutrients != null)
                    foreach (Nutrient nutrient in food.AvailableNutrients)
                        model.Nutrients.Add(AutoMapper.Mapper.Map<NutrientViewModel>(nutrient));
                if (food.AvailableServings != null)
                    foreach (Serving serving in food.AvailableServings)
                        model.Servings.Add(AutoMapper.Mapper.Map<ServingViewModel>(serving));
            }
            return food;
        }

        // POST /api/food
        [CustomAuthorize]
        public HttpResponseMessage Post(IList<SaveFoodViewModel> value)
        {
            IList<Meal> meals = new List<Meal>();
            foreach (SaveFoodViewModel saveFoodViewModel in value)
            {
                Meal meal = meals.FirstOrDefault(t => t.MealDate.Equals(saveFoodViewModel.Date));
                if (meal != null)
                    meal.Foods.Add(AutoMapper.Mapper.Map<FoodEntry>(saveFoodViewModel));
                else
                {
                    meal = new Meal()
                    {
                        Foods = new List<FoodEntry>(),
                        MealDate = saveFoodViewModel.Date
                    };
                    meal.Foods.Add(AutoMapper.Mapper.Map<FoodEntry>(saveFoodViewModel));
                    meals.Add(meal);
                }
            }
            if(mealRepository.SaveMeals(meals))
                return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
            else
                return new HttpResponseMessage(System.Net.HttpStatusCode.Conflict);
        }

        // PUT /api/food/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/food/5
        public void Delete(int id)
        {
        }
    }
}
