using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Repository;
using Domain.Entities;

namespace Domain.Factory.Interfaces
{
    public interface IContestFactory
    {
        IList<Contest> RetrieveContests(IContestRepository repository);
        IList<ContestEvent> RetrieveContestEvents(int contestId, IContestRepository repository);
    }
}
