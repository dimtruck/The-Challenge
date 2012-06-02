using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Factory.Interfaces;
using Domain.Repository;
using Domain.Entities;
using Domain.Cache;

namespace Domain.Factory
{
    public class ExerciseFactory : IExerciseFactory
    {
        public IList<Entities.Event> RetrieveExercises(IExerciseRepository repository)
        {
            //retrieve from cache
            IList<Event> exercises = ExerciseClient.RetrieveExercises();
            if (exercises == null)
            {
                //retrieve from database
                exercises = repository.RetrieveExercises();
                //save to cache
                ExerciseClient.SaveExercises(exercises);
            }

            return exercises;

        }
    }
}
