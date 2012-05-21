using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Domain.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private SqlConnection connection;

        public ProfileRepository(String connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public IList<CurrentStatistic> RetrieveProfile()
        {
            List<CurrentStatistic> results = new List<CurrentStatistic>();
            using (this.connection)
            {
                this.connection.Open();
                results.AddRange(RetrieveMax());
            }

            return results;
        }

        public IList<ContestEventGoal> RetrieveGoals()
        {
            IList<ContestEventGoal> results = new List<ContestEventGoal>();
            using (this.connection)
            {
                this.connection.Open();
                var tempResult = this.connection.Query(@"select	a.EventContestGoalId, a.GoalWeight, a.GoalReps, a.GoalTime, a.GoalDistance, c.ContestId, c.ContestName, c.ContestDate,c.ContestDetails, c.ContestPlace, d.EventId, d.EventName, b.eventGoalTypeId
                                                        from	thechallenge.dimitryushakov.eventcontestgoal a,
		                                                        thechallenge.dimitryushakov.contestexercise b,
		                                                        thechallenge.dimitryushakov.contest c,
		                                                        thechallenge.dimitryushakov.[event] d
                                                        where	a.contestexerciseid = b.contestexerciseid
                                                        and		b.contestid = c.contestid
                                                        and     b.eventid = d.eventid
                                                        and		c.contestdate > current_timestamp
                                                        order by c.contestdate ASC");
                foreach (var entry in tempResult)
                {
                    results.Add(new ContestEventGoal()
                    {
                        Contest = new Contest()
                        {
                            ContestDate = entry.ContestDate,
                            ContestDetails = entry.ContestDetails,
                            ContestId = entry.ContestId,
                            ContestName = entry.ContestName,
                            ContestPlace = entry.ContestPlace
                        },
                        Event = new Event(){
                            EventId = entry.EventId,
                            EventName = entry.EventName
                        },
                        Distance = entry.GoalDistance,
                        Id = entry.EventContestGoalId,
                        Reps = entry.GoalReps,
                        Time = entry.GoalTime != null?TimeSpan.FromMilliseconds(entry.GoalTime):new TimeSpan(0),
                        Weight = entry.GoalWeight,
                        GoalTypeId = entry.eventGoalTypeId
                    });
                }
            }

            return results;
        }

        #region "Private Methods"

        private IList<CurrentStatistic> RetrieveMax()
        {
            return this.connection.Query<CurrentStatistic>(@"select	event.eventName as Exercise, max(workout.workoutDate) as LastExecuted, cast(b.max_weight as nvarchar) as Result, 'MAX_LIFT' as EventGoalType 
                                                            from	thechallenge.dimitryushakov.[workout] workout,
                                                                   (
                                                            			select	max(c.[weight]) as max_weight, a.eventid as eventId
                                                            			from	thechallenge.dimitryushakov.[contestexercise] a,
                                                            					thechallenge.dimitryushakov.[event] b,
                                                            					thechallenge.dimitryushakov.[workout] c
                                                            			where	a.eventgoaltypeid = 1
                                                            			and		a.eventId = b.eventId
                                                            			and		c.eventId = b.eventId
                                                            			group by a.eventId
                                                            		) b, 
                                                                   thechallenge.dimitryushakov.[event] event 
                                                            where  event.eventId = workout.eventId 
                                                            and    workout.eventId = b.eventId 
                                                            and	workout.[weight] = b.max_weight
                                                            group by event.eventName,b.max_weight").ToList();

        }


        #endregion
    }
}
