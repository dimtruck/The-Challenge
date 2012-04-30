using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IWorkoutRepository
    {
        bool SaveWorkout(Workout workout);

        IList<DateTime> GetWorkoutDates();
        Workout GetWorkout(DateTime entryDate);
    }
}
