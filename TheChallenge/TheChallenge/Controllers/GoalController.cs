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

            if (contestEventGoalList != null)
                foreach (ContestEventGoal contestEventGoal in contestEventGoalList)
                    contestEventViewModelList.Add(AutoMapper.Mapper.Map<ContestEventGoalViewModel>(contestEventGoal));

            return contestEventViewModelList;
        }
    }
}
