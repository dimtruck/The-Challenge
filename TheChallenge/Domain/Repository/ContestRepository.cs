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
    public class ContestRepository : IContestRepository
    {
        private SqlConnection connection;

        public ContestRepository(String connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public IList<Contest> RetrieveContests()
        {
            IList<Contest> results;
            using(this.connection)
	        {
                this.connection.Open();
                results = this.connection.Query<Contest>("SELECT * FROM [TheChallenge].[DimitryUshakov].[Contest]").ToList();
		 
	        }

            return results;
        }


        public IList<ContestEvent> RetrieveContestEvents(int contestId)
        {
            IList<ContestEvent> results;
            using (this.connection)
            {
                this.connection.Open();
                results = this.connection.Query<ContestEvent>(@"SELECT  b.eventName as EventName, c.EventGoalTypeName as EventType, c.EventGoalTypeDesc as EventDescription, a.EventGoal as EventGoal " +
                                                                "FROM   [TheChallenge].[DimitryUshakov].[ContestExercise] a, [TheChallenge].[DimitryUshakov].[Event] b, [TheChallenge].[DimitryUshakov].[EventGoalType] c " +
                                                                "WHERE  a.ContestId = @contestId " +
                                                                "AND    a.EventId = b.EventId " +
                                                                "AND    a.EventGoalTypeId = c.EventGoalTypeId", new { contestId = contestId }).ToList();

            }

            return results;
        }
    }
}
