using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TheChallenge.Models;
using Domain.Entities;
using Domain.Repository;
using TheChallenge.Helpers;

namespace TheChallenge.Controllers
{
    public class GoalController : ApiController
    {
        private readonly IProfileRepository repository;

        public GoalController(IProfileRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/goal
        [CustomAuthorize]
        public IList<ContestEventGoalViewModel> Get()
        {
            IList<ContestEventGoal> contestEventGoalList = repository.RetrieveGoals();
            IList<ContestEventGoalViewModel> contestEventViewModelList = new List<ContestEventGoalViewModel>();

            foreach (ContestEventGoal contestEventGoal in contestEventGoalList)
            {
                contestEventViewModelList.Add(AutoMapper.Mapper.Map<ContestEventGoalViewModel>(contestEventGoal));
            }

            return contestEventViewModelList;
        }

        // GET /api/goal/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/goal
        public void Post(string value)
        {
        }

        // PUT /api/goal/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/goal/5
        public void Delete(int id)
        {
        }
    }
}
