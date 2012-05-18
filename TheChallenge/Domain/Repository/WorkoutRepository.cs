using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using System.Data.SqlClient;
using Dapper;

namespace Domain.Repository
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private SqlConnection connection;

        public WorkoutRepository(String connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public bool SaveWorkout(Workout workout)
        {
            bool result = false;
            using (this.connection)
            {
                this.connection.Open();
                int numberOfEntries = 0;
                foreach (ExerciseEntry exerciseEntry in workout.ExerciseEntries)
                {
                    numberOfEntries = numberOfEntries + connection.Execute(@"insert into [TheChallenge].[DimitryUshakov].[Workout] values(@WorkoutDate,@EventId, @Reps,@Weight,@EventTime,@Distance)", new { WorkoutDate = workout.WorkoutDate, EventId = exerciseEntry.ExerciseId, Reps = exerciseEntry.Reps, Weight = exerciseEntry.Weight, EventTime = exerciseEntry.Time.HasValue?exerciseEntry.Time.Value.TotalMilliseconds:0, Distance = exerciseEntry.Distance });
                }
                if (numberOfEntries > 0)
                    result = true;
            }

            return result;
        }


        public IList<DateTime> GetWorkoutDates()
        {
            //TODO: refactor to take out meal and workout and separate
            IList<DateTime> results;
            using (this.connection)
            {
                this.connection.Open();
                results = this.connection.Query<DateTime>(@"select distinct CAST(MealDate as date) from thechallenge.dimitryushakov.[meal]
                                                            union
                                                            SELECT distinct WorkoutDate FROM [TheChallenge].[DimitryUshakov].[Workout]").ToList();
            }

            return results;
        }


        public Workout GetWorkout(DateTime entryDate)
        {
            Workout workout = new Workout(){
                ExerciseEntries = new List<ExerciseEntry>(),
                WorkoutDate = entryDate
            };
            using (this.connection)
            {
                this.connection.Open();
                var results = this.connection.Query("SELECT * FROM [TheChallenge].[DimitryUshakov].[Workout] a,[TheChallenge].[DimitryUshakov].[Event] b WHERE WorkoutDate=@EntryDate AND a.EventId = b.EventId", new { EntryDate = entryDate }).ToList();

                foreach(var result in results){
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

            return workout;
        }
    }
}
