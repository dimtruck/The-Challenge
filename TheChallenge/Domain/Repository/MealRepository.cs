using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Domain.Entities;
using Dapper;

namespace Domain.Repository
{
    public class MealRepository : IMealRepository
    {
        private SqlConnection connection;
        private String connectionString;

        public MealRepository(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool SaveMeals(IList<Meal> meals)
        {
            int numberOfEntries = 0;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                //enter meal
                foreach (Meal meal in meals)
                {
                    numberOfEntries = connection.Execute(@"insert into [TheChallenge].[DimitryUshakov].[Meal] values(@Date,@FoodId, @ServingSize,@ServingId)", meal.Foods);
                }
            }

            return numberOfEntries > 0;
        }


        public IList<Meal> GetMeals(DateTime entryDate)
        {
            Workout workout = new Workout()
            {
                ExerciseEntries = new List<ExerciseEntry>(),
                WorkoutDate = entryDate
            };
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                //add foodentry with all data that is also populated from food_repository
                var results = this.connection.Query(@"select MealDate as [Date], FoodId, ServingSize, ServingType, msre_desc as ServingDesc from thechallenge.dimitryushakov.[meal] a,
                                                        thechallenge.dimitryushakov.food_des b,
                                                        thechallenge.dimitryushakov.food_weight c
                                                        where cast(mealdate as date) = @EntryDate
                                                        and cast(b.ndb_no as int) = a.foodid
                                                        and b.ndb_no = c.nbd_no
                                                        and cast(c.seq as int) = a.servingtype", new { EntryDate = entryDate }).ToList();

                foreach (var result in results)
                {
                    workout.ExerciseEntries.Add(new ExerciseEntry()
                    {
                        Time = TimeSpan.FromMilliseconds(result.EntryTime),
                        Distance = result.Distance,
                        ExerciseId = result.EventId,
                        Reps = result.Reps,
                        Weight = result.Weight,
                        Name = result.EventName,
                    });
                }

            }

            return null;
        }
    }
}
