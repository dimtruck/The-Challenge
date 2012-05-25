using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TheChallenge.Models;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Helpers;

namespace TheChallenge.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IProfileRepository repository;

        public ProfileController(IProfileRepository repository)
        {
            this.repository = repository;
        }

        // GET /api/profile/current
        [CustomAuthorize]
        public IList<CurrentLiftsViewModel> Get(String current)
        {
/***
	- RETURN MAX LIFT
		- get all events from contestexercise for contestid and eventGoalType = 'MAX'
			- for each event
				- get all entries in workout table sorted by WEIGHT desc
					- get first and create WEIGHT'x'REPS tag and return
	- RETURN LIFT FOR REPS
		- get all events from contestexercise for contestid and eventtype = 'WEIGHT' and weight = eventGoal
			- for each event
				- get all entries in workout table sorted by REPS desc
					- get first and create WEIGHT'x'REPS tag and return
	- RETURN IS COMPLETED
		- get all events from contestexercise for contestid and eventtype = 'REPTIME'
			- for each event
				- get all entries in workout table sorted by IS_COMPLETED desc
					- get first and create EVENT_NAME tag and return
	- RETURN LIFT FOR REPS TIMED
		- get all events from contestexercise for contestid and eventtype = 'WEIGHTTIME' and weight = eventGoal[0] and time = eventGoal[1]
			- for each event
				- get all entries in workout table sorted by REPS desc
					- get first and create WEIGHT'x'REPS'x'TIME tag and return
	- RETURN MAX SPEED
		- get all events from contestexercise for contestid and eventtype = 'DISTANCE' and distance = eventGoal
			- for each event
				- get all entries in workout table sorted by TIME asc
					- get first and create DISTANCE'x'TIME tag and return
	- RETURN MAX SPEED WITH WEIGHT
		- get all events from contestexercise for contestid and eventtype = 'WEIGHTDISTANCE' and distance = eventGoal[0] and weight = eventGoal[1]
			- for each event
				- get all entries in workout table sorted by TIME asc
					- get first and create DISTANCE'x'WEIGHT'x'TIME tag and return
 */
            IList<CurrentStatistic> currentStatisticList = repository.RetrieveProfile();
            IList<CurrentLiftsViewModel> currentLiftsList = new List<CurrentLiftsViewModel>();

            if (currentStatisticList != null)
                foreach (CurrentStatistic statistic in currentStatisticList)
                    currentLiftsList.Add(AutoMapper.Mapper.Map<CurrentLiftsViewModel>(statistic));

            return currentLiftsList;
        }
    }
}
