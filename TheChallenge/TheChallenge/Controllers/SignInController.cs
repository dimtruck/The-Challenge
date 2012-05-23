using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TheChallenge.Models;
using Domain.Repository;
using TheChallenge.Helpers.Encryption;
using Domain.Entities;

namespace TheChallenge.Controllers
{
    public class SignInController : ApiController
    {
        private IUserRepository repository;

        public SignInController(IUserRepository repository)
        {
            this.repository = repository;
        }

        // POST /api/signin
        public HttpResponseMessage Post(LoginViewModel value)
        {
            bool IsValidated = false;
            User user = repository.UserExists(value.UserName);
            if(user != null && value.Password.Equals(Crypto.DecryptStringAES(user.Password)))
                IsValidated = true;
            
            if (IsValidated)
            {
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                };

                response.Headers.Add("TC-Authorization", Crypto.EncryptStringAES(value.UserName + ":" + Crypto.EncryptStringAES(value.Password)));
                response.Headers.Age = new TimeSpan(DateTime.Now.AddHours(1).Ticks);
                return response;
            }
            else
            {
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }
        }
    }
}
