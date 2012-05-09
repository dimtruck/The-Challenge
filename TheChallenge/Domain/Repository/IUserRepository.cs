using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        User GetUser(int userId);

        User UserExists(String userName);
        bool AuthenticateUser(String userName, String password);
    }
}
