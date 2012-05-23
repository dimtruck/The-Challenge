using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Domain.Repository;
using Domain.Entities;
using TheChallenge.Models;
using TheChallenge.Helpers.Encryption;
using TheChallenge.Controllers;
using System.Net.Http;
using System.Net;

namespace UnitTest.APIs
{
    [TestFixture]
    public class SignOnApiTest
    {
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private readonly LoginViewModel loginViewModel = new LoginViewModel()
        {
            Password = "test",
            UserName = "test"
        };

        [TearDown]
        public void TearDown()
        {
            userRepositoryMock.VerifyAll();
        }

        [Test]
        public void SuccessfullyLoginTest()
        {
            userRepositoryMock.Setup(t => t.UserExists(It.IsAny<String>())).Returns(new User()
            {
                IsValid = true,
                Name = "test",
                Password = Crypto.EncryptStringAES(loginViewModel.Password)
            });

            SignInController controller = new SignInController(userRepositoryMock.Object);
            var results = controller.Post(loginViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.OK, results.StatusCode);
            Assert.IsNotNull(results.Headers);
            Assert.IsTrue(results.Headers.Contains("TC-Authorization"));
            Assert.IsTrue(results.Headers.Age <= new TimeSpan(DateTime.Now.AddHours(1).Ticks));
        }

        [Test]
        public void InvalidUserLoginTest()
        {
            User user = null;
            userRepositoryMock.Setup(t => t.UserExists(It.IsAny<String>())).Returns(user);

            SignInController controller = new SignInController(userRepositoryMock.Object);
            var results = controller.Post(loginViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.Unauthorized, results.StatusCode);
            Assert.IsEmpty(results.Headers);
        }

        [Test]
        public void InvalidPasswordLoginTest()
        {
            userRepositoryMock.Setup(t => t.UserExists(It.IsAny<String>())).Returns(new User()
            {
                IsValid = true,
                Name = "test",
                Password = Crypto.EncryptStringAES("different password")
            });

            SignInController controller = new SignInController(userRepositoryMock.Object);
            var results = controller.Post(loginViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.Unauthorized, results.StatusCode);
            Assert.IsEmpty(results.Headers);
        }


        
    }
}
