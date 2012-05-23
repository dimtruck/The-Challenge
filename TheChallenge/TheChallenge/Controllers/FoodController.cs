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
            if (foodList != null)
                foreach (Food food in foodList)
                    foodModelList.Add(AutoMapper.Mapper.Map<FoodViewModel>(food));
            return foodModelList;
        }

        // GET /api/food/5
        public FoodViewModel Get(int id)
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
            FoodViewModel model = new FoodViewModel();
            Food food = repository.RetrieveCompleteFood(id);
            if (food != null)
            {
                model = AutoMapper.Mapper.Map<FoodViewModel>(food);
                if (food.AvailableNutrients != null)
                {
                    model.Nutrients = new List<NutrientViewModel>();
                    foreach (Nutrient nutrient in food.AvailableNutrients)
                        model.Nutrients.Add(AutoMapper.Mapper.Map<NutrientViewModel>(nutrient));
                }
                if (food.AvailableServings != null)
                {
                    model.Servings = new List<ServingViewModel>();
                    foreach (Serving serving in food.AvailableServings)
                        model.Servings.Add(AutoMapper.Mapper.Map<ServingViewModel>(serving));
                }
            }
            return model;
        }

        // POST /api/food
        [CustomAuthorize]
        public HttpResponseMessage Post(IList<SaveFoodViewModel> value)
        {
            if(value == null)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "no valid request content present" };
            if (value.FirstOrDefault(t => t.Date == null || t.Date == DateTime.MinValue || t.Date == DateTime.MaxValue) != null)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "no valid date present" };
            if (value.FirstOrDefault(t => t.FoodId == 0) != null)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = "no foods present" };
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
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError) { ReasonPhrase = "unable to save food" };
        }
    }
}
