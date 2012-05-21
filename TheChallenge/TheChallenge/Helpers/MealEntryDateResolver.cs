using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using AutoMapper;
using System.Text;

namespace TheChallenge.Helpers
{
    public class MealEntryDateResolver : ValueResolver<FoodEntry, string>
    {
        protected override string ResolveCore(FoodEntry source)
        {
            if (source.Date != null && source.Date > DateTime.MinValue && source.Date < DateTime.MaxValue)
            {
                return source.Date.Month + "/" + source.Date.Day + "/" + source.Date.Year + " " + source.Date.Hour + ":" + source.Date.Minute;
            }
            return string.Empty;
        }
    }
}