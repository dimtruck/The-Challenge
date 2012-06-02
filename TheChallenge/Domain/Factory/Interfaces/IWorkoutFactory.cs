using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Domain.Repository;

namespace Domain.Factory.Interfaces
{
    public interface IWorkoutFactory
    {
        IList<DateTime> GetWorkoutDates(IWorkoutRepository repository);
        Workout GetWorkout(DateTime entryDate, IWorkoutRepository repository);
        bool SaveWorkout(Workout workout, IWorkoutRepository repository);
    }
}
