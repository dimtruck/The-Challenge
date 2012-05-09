using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using TheChallenge.Controllers;
using TheChallenge.Models;
using System.Net;
using Domain.Repository;
using Moq;
using Domain.Entities;
using TheChallenge.Helpers.Encryption;

namespace Test
{
    class AccountTest : nspec
    {
        void describe_Register()
        {
            context["When Registering"] = () =>
                {
                    beforeEach = () => RegisterSetup();
                    it["Should have TC_Authorization header"] = () =>
                    {
                        controller.Post(register).should(t => t.Headers.Contains("TC-Authorization"));
                    };
                    it["Should have Cache header less than or equal to 1 hour in advance"] = () =>
                    {
                        controller.Post(register).Headers.Age.is_less_or_equal_to(new TimeSpan(DateTime.Now.AddHours(1).Ticks) );
                    };
                    it["Should have response status code of created"] = () =>
                    {
                        controller.Post(register).StatusCode.should_be(HttpStatusCode.Created);
                    };
                    it["Should have called authenticate user and returned true"] = () =>
                    {
                        controller.Post(register).StatusCode.should_be(HttpStatusCode.Created);
                        userRepositoryMock.Verify<Boolean>(t => t.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>()), Times.Once());
                    };
                };

            context["When Failing To Register"] = () =>
            {
                beforeEach = () => RegisterBadSetup();
                
                it["Should not have TC_Authorization header"] = () =>
                {
                    controller.Post(register).should(t => !t.Headers.Contains("TC-Authorization"));
                };
                it["Should not have Age header less than or equal to 1 hour in advance"] = () =>
                {
                    controller.Post(register).Headers.Age.HasValue.should_be_false();
                };
                it["Should have response status code of bad request"] = () =>
                {
                    controller.Post(register).StatusCode.should_be(HttpStatusCode.BadRequest);
                };
                it["Should have called authenticate user"] = () =>
                {
                    controller.Post(register).StatusCode.should_be(HttpStatusCode.BadRequest);
                    userRepositoryMock.Verify<Boolean>(t => t.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>()), Times.Once());
                };
            };
        }

        void describe_Login()
        {
            context["When Logging In"] = () =>
            {
                beforeEach = () => LoginSetup();
                it["Should have TC_Authorization header"] = () =>
                {
                    signInController.Post(login).should(t => t.Headers.Contains("TC-Authorization"));
                };
                it["Should have Cache header less than or equal to 1 hour in advance"] = () =>
                {
                    signInController.Post(login).Headers.Age.is_less_or_equal_to(new TimeSpan(DateTime.Now.AddHours(1).Ticks));
                };
                it["Should have response status code of no content"] = () =>
                {
                    signInController.Post(login).StatusCode.should_be(HttpStatusCode.NoContent);
                };
                it["Should have called user exists and returned true"] = () =>
                {
                    signInController.Post(login).StatusCode.should_be(HttpStatusCode.NoContent);
                    userRepositoryMock.Verify<User>(t => t.UserExists(It.IsAny<String>()), Times.Once());
                };
            };

            context["When Failing To Log In"] = () =>
            {
                beforeEach = () => LoginBadSetup();

                it["Should not have TC_Authorization header"] = () =>
                {
                    signInController.Post(login).should(t => !t.Headers.Contains("TC-Authorization"));
                };
                it["Should not have Age header less than or equal to 1 hour in advance"] = () =>
                {
                    signInController.Post(login).Headers.Age.HasValue.should_be_false();
                };
                it["Should have response status code of unauthorized"] = () =>
                {
                    signInController.Post(login).StatusCode.should_be(HttpStatusCode.Unauthorized);
                };
                it["Should have called user exists user"] = () =>
                {
                    signInController.Post(login).StatusCode.should_be(HttpStatusCode.Unauthorized);
                    userRepositoryMock.Verify<User>(t => t.UserExists(It.IsAny<String>()), Times.Once());
                };
            };
        }

        private AccountController controller;
        private SignInController signInController;
        private RegisterViewModel register = new RegisterViewModel()
        {
            UserName = "username",
            Password = "password"
        };
        private LoginViewModel login = new LoginViewModel()
        {
            UserName = "username",
            Password = "password"
        };


        private Moq.Mock<IUserRepository> userRepositoryMock;

        private void RegisterSetup()
        {
            userRepositoryMock = new Moq.Mock<IUserRepository>();
            userRepositoryMock.Setup(t => t.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            controller = new AccountController(userRepositoryMock.Object);
        }

        private void RegisterBadSetup()
        {
            userRepositoryMock = new Moq.Mock<IUserRepository>();
            userRepositoryMock.Setup(t => t.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            controller = new AccountController(userRepositoryMock.Object);
        }

        private void LoginSetup()
        {
            userRepositoryMock = new Moq.Mock<IUserRepository>();
            String test = Crypto.EncryptStringAES(login.Password);
            userRepositoryMock.Setup(t => t.UserExists(It.IsAny<String>())).Returns(new User() { Password = test, Name = login.UserName});
            signInController = new SignInController(userRepositoryMock.Object);
        }

        private void LoginBadSetup()
        {
            userRepositoryMock = new Moq.Mock<IUserRepository>();
            User user = null;
            userRepositoryMock.Setup(t => t.UserExists(It.IsAny<String>())).Returns(user);
            signInController = new SignInController(userRepositoryMock.Object);
        }
    }
}
