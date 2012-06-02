using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.Repository;
using Domain.Cache;
using Domain.Factory.Interfaces;

namespace Domain.Factory
{
    public class ContestFactory :IContestFactory
    {
        public IList<Contest> RetrieveContests(IContestRepository repository)
        {
            //retrieve from cache
            IList<Contest> contests = ContestClient.RetrieveContests();
            if (contests == null)
            {
                //retrieve from database
                contests = repository.RetrieveContests();
                //save to cache
                ContestClient.SaveContests(contests);
            }

            return contests;
        }

        public IList<ContestEvent> RetrieveContestEvents(int contestId, IContestRepository repository)
        {
            IList<ContestEvent> contestEvents = ContestClient.RetrieveContestEvents(contestId);
            if (contestEvents == null)
            {
                contestEvents = repository.RetrieveContestEvents(contestId);
                ContestClient.SaveContestEvents(contestEvents, contestId);
            }

            return contestEvents;
        }
    }
}
