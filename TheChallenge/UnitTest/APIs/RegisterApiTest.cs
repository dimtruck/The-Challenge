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
    public class RegisterApiTest
    {
        private Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
        private readonly RegisterViewModel registerViewModel = new RegisterViewModel()
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
        public void SuccessfullySignOutTest() {
            AccountController controller = new AccountController(userRepositoryMock.Object);
            controller.Request = new HttpRequestMessage();
            var results = controller.Get() as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.OK, results.StatusCode);
            Assert.IsEmpty(results.Headers);
        }

        [Test]
        public void SuccessfullyRegisterTest() {
            userRepositoryMock.Setup(t => t.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>())).Returns(true);

            AccountController controller = new AccountController(userRepositoryMock.Object);
            var results = controller.Post(registerViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.Created, results.StatusCode);
            Assert.IsNotNull(results.Headers);
            Assert.IsTrue(results.Headers.Contains("TC-Authorization"));
            Assert.IsTrue(results.Headers.Age <= new TimeSpan(DateTime.Now.AddHours(1).Ticks));
        }

        [Test]
        public void UnsuccessfullyRegisterTest()
        {
            userRepositoryMock.Setup(t => t.AuthenticateUser(It.IsAny<String>(), It.IsAny<String>())).Returns(false);

            AccountController controller = new AccountController(userRepositoryMock.Object);
            var results = controller.Post(registerViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.IsEmpty(results.Headers);
        }

    }
}
