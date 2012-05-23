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
using TheChallenge.Helpers;

namespace UnitTest.APIs
{
    [TestFixture]
    class GoalApiTest
    {
        private Mock<IProfileRepository> profileRepositoryMock = new Mock<IProfileRepository>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<ContestEvent, ContestEventViewModel>()
                .ForMember(dest => dest.EventGoal, opt => opt.ResolveUsing<ContestEventDataResolver>());
            AutoMapper.Mapper.CreateMap<ContestEventGoal, ContestEventGoalViewModel>()
                .ForMember(dest => dest.Result, opt => opt.ResolveUsing<GoalDataResolver>());
        }

        [TearDown]
        public void TearDown()
        {
            profileRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllGoalsTest()
        {
            profileRepositoryMock.Setup(t => t.RetrieveGoals()).Returns(new List<ContestEventGoal>(){
                new ContestEventGoal(){ Event = new Event(){ EventName = "test"}, Id = 1},
                new ContestEventGoal() { Event = new Event(){ EventName = "test2"}, Id = 2},
                new ContestEventGoal() { Event = new Event(){ EventName = "test3"}, Id = 3}
            });

            GoalController controller = new GoalController(profileRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<ContestEventGoalViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("test", results.First(t => t.Id == 1).Event.Name);
        }

        [Test]
        public void GetAllGoalsNoneReturnTest()
        {
            IList<ContestEventGoal> contestEventGoals = null;
            profileRepositoryMock.Setup(t => t.RetrieveGoals()).Returns(contestEventGoals);

            GoalController controller = new GoalController(profileRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<ContestEventGoalViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }
    }
}
