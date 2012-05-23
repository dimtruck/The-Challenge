using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain;
using Domain.Entities;
using TheChallenge.Models;
using Domain.Repository;
using TheChallenge.Helpers;

namespace TheChallenge.Controllers
{
    public class ContestController : ApiController
    {
        private readonly IContestRepository repository;

        public ContestController(IContestRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/contest
        [CustomAuthorize]
        public IEnumerable<ContestViewModel> Get()
        {
            IList<Contest> contestList = this.repository.RetrieveContests();
            IList<ContestViewModel> contestViewModelList = new List<ContestViewModel>();
            if (contestList != null)
                foreach (Contest contest in contestList)
                    contestViewModelList.Add(AutoMapper.Mapper.Map<ContestViewModel>(contest));

            return contestViewModelList;
        }

        // GET /api/contest/5
        [CustomAuthorize]
        public IList<ContestEventViewModel> Get(int id)
        {
            //get all events for that contest
            IList<ContestEvent> contestEventList = this.repository.RetrieveContestEvents(id);
            IList<ContestEventViewModel> contestEventViewModelList = new List<ContestEventViewModel>();
            if (contestEventList != null)
                foreach (ContestEvent contestEvent in contestEventList)
                    contestEventViewModelList.Add(AutoMapper.Mapper.Map<ContestEventViewModel>(contestEvent));
            return contestEventViewModelList;
        }
    }
}
