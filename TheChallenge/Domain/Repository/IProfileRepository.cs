using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IProfileRepository
    {
        IList<CurrentStatistic> RetrieveProfile();
        IList<ContestEventGoal> RetrieveGoals();
    }
}
