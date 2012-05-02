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
    public class FoodController : ApiController
    {
        private readonly IFoodRepository repository;

        public FoodController(IFoodRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/food
        public IEnumerable<FoodViewModel> Get()
        {
            IList<Food> foodList = this.repository.RetrieveFoodNames();
            IList<FoodViewModel> foodModelList = new List<FoodViewModel>();
            foreach (Food food in foodList)
            {
                foodModelList.Add(AutoMapper.Mapper.Map<FoodViewModel>(food));
            }
            return foodModelList;
        }

        // GET /api/food/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/food
        public void Post(string value)
        {
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
