using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using Domain.Entities;

namespace Domain.Repository
{
    public class UserRepository : IUserRepository
    {
        private SqlConnection connection;

        public UserRepository(String connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public Entities.User GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(string userName, string encryptedPassword)
        {
            //authenticate user here
            int insertValid = 0;
            using (this.connection)
            {
                this.connection.Open();
                insertValid = this.connection.Execute(@"insert into [TheChallenge].[DimitryUshakov].[User] values(@userName,@encryptedPassword, '1',1)", new { userName = userName, encryptedPassword = encryptedPassword });
            }

            return insertValid > 0;
        }

        public User UserExists(string userName)
        {
            using (this.connection)
            {
                this.connection.Open();
                return this.connection.Query<User>("select UserName as [Name], Password from thechallenge.dimitryushakov.[user] where UserName = @userName", new { userName = userName }).FirstOrDefault();
            }
        }
    }
}
