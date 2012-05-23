﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TheChallenge.Models;
using Domain.Repository;
using TheChallenge.Helpers.Encryption;

namespace TheChallenge.Controllers
{
    public class AccountController : ApiController
    {
        private IUserRepository repository;

        public AccountController(IUserRepository repository)
        {
            this.repository = repository;
        }

        public HttpResponseMessage Get()
        {
            if(Request.Headers != null)
                this.Request.Headers.Remove("TC-Authorization");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        // POST /api/account
        public HttpResponseMessage Post(RegisterViewModel value)
        {
            String password = Crypto.EncryptStringAES(value.Password);
            if (repository.AuthenticateUser(value.UserName, password))
            {
                //registered
                //set the tc-authorization
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Created
                };

                response.Headers.Add("TC-Authorization", Crypto.EncryptStringAES(value.UserName + ":" + value.Password));
                response.Headers.Age = new TimeSpan(DateTime.Now.AddHours(1).Ticks);
                return response;
            }
            else
            {
                //not registered
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            }
        }
    }
}