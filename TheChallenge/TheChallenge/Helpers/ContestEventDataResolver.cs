using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using AutoMapper;
using System.Text;

namespace TheChallenge.Helpers
{
    public class ContestEventDataResolver : ValueResolver<ContestEvent, string>
    {
        protected override string ResolveCore(ContestEvent source)
        {
            if (String.IsNullOrEmpty(source.EventType))
                return string.Empty;
            if (source.EventType.Equals("MAX"))
                return "Lift as much as possible.";
            if (source.EventType.Equals("WEIGHTDISTANCE"))
            {
                String[] extraInfo = source.EventGoal.Split('|');
                return String.Format("Complete as fast as possible with {0} pounds for {1}", extraInfo[0], extraInfo[1]);
            }
            if (source.EventType.Equals("WEIGHTTIME"))
            {
                String[] extraInfo = source.EventGoal.Split('|');
                return String.Format("Lift {0} pounds as many times as possible in {1}", extraInfo[0], extraInfo[1]);
            }
            if (source.EventType.Equals("DISTANCE"))
            {
                return String.Format("Complete as fast as possible with the following weights: {0}", source.EventGoal);
            }

            return string.Empty;
        }
    }
}