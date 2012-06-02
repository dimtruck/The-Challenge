using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Factory.Interfaces;
using Domain.Cache;
using Domain.Entities;

namespace Domain.Factory
{
    public class ProfileFactory : IProfileFactory
    {
        public IList<Entities.ContestEventGoal> RetrieveGoals(Repository.IProfileRepository repository)
        {
            //retrieve from cache
            IList<ContestEventGoal> goals = ProfileClient.RetrieveGoals();
            if (goals == null)
            {
                //retrieve from database
                goals = repository.RetrieveGoals();
                //save to cache
                ProfileClient.SaveGoals(goals);
            }

            return goals;
        }

        public IList<Entities.CurrentStatistic> RetrieveProfile(Repository.IProfileRepository repository)
        {
            //retrieve from cache
            IList<CurrentStatistic> profile = ProfileClient.RetrieveProfile();
            if (profile == null)
            {
                //retrieve from database
                profile = repository.RetrieveProfile();
                //save to cache
                ProfileClient.SaveProfile(profile);
            }

            return profile;
        }
    }
}
