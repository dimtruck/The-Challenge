using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.Repository;

namespace Domain.Factory.Interfaces
{
    public interface IExerciseFactory
    {
        IList<Event> RetrieveExercises(IExerciseRepository repository);
    }
}
