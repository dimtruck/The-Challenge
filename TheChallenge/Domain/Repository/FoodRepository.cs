using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using System.Data.SqlClient;
using Dapper;

namespace Domain.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private SqlConnection connection;

        public FoodRepository(String connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public IList<Food> RetrieveFoodNames()
        {
            IList<Food> results;
            using (this.connection)
            {
                this.connection.Open();
                results = this.connection.Query<Food>("SELECT NDB_No as Id, Long_Desc as Name FROM [TheChallenge].[DimitryUshakov].[Food_Des]").ToList();

            }

            return results;
        }
    }
}
