using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.Repository;

namespace Domain.Factory.Interfaces
{
    public interface IProfileFactory
    {
        IList<ContestEventGoal> RetrieveGoals(IProfileRepository repository);
        IList<CurrentStatistic> RetrieveProfile(IProfileRepository repository);
    }
}
