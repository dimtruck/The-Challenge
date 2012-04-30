using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using AutoMapper;

namespace TheChallenge.Helpers
{
    public class GoalDataResolver : ValueResolver<ContestEventGoal, String>
    {

        protected override string ResolveCore(ContestEventGoal source)
        {
            //max
            if (source.GoalTypeId == 1)
                return source.Weight.ToString() + " pounds";
            if (source.GoalTypeId == 2)
                return source.Reps.ToString() + " repetitions";
            if (source.GoalTypeId == 3)
                return source.Time.ToString();
            if (source.GoalTypeId == 4)
                return source.Reps.ToString() +" repetitions";
            if (source.GoalTypeId == 5)
                return source.Time.ToString();
            if (source.GoalTypeId == 6)
                return source.Time.ToString();

            return null;
                       
        }
    }
}