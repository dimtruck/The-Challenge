using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using System.Data.SqlClient;
using Dapper;

namespace Domain.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private SqlConnection connection;

        public ExerciseRepository(String connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public IList<Event> RetrieveExercises()
        {
            IList<Event> results;
            using(this.connection)
	        {
                this.connection.Open();
                results = this.connection.Query<Event>("SELECT * FROM [TheChallenge].[DimitryUshakov].[Event]").ToList();
		 
	        }

            return results;
        }
    }
}
