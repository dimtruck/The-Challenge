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
    class ProfileApiTest
    {
        private Mock<IProfileRepository> profileRepositoryMock = new Mock<IProfileRepository>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<CurrentStatistic, CurrentLiftsViewModel>()
                .ForMember(dest => dest.DateLifted, opt => opt.MapFrom(src => src.LastExecuted))
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Exercise))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventGoalType))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result));
        }

        [TearDown]
        public void TearDown()
        {
            profileRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetCurrentStatsTest()
        {
            profileRepositoryMock.Setup(t => t.RetrieveProfile()).Returns(new List<CurrentStatistic>(){
                new CurrentStatistic(){
                    EventGoalType = "MAX",
                    Exercise = "Squats",
                    LastExecuted = DateTime.Now,
                    Result = "150x5"
                },
                new CurrentStatistic(){
                    EventGoalType = "MAX",
                    Exercise = "Golf",
                    LastExecuted = DateTime.Now,
                    Result = "8 pins"
                },
                new CurrentStatistic(){
                    EventGoalType = "MAX",
                    Exercise = "Run",
                    LastExecuted = DateTime.Now,
                    Result = "1 mile"
                }
            });

            ProfileController controller = new ProfileController(profileRepositoryMock.Object);
            var results = controller.Get("test") as IEnumerable<CurrentLiftsViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("Golf", results.First(t => t.Result.Equals("8 pins")).Event);
        }

        [Test]
        public void GetCurrentStatsNoneTest()
        {
            IList<CurrentStatistic> stats = null;
            profileRepositoryMock.Setup(t => t.RetrieveProfile()).Returns(stats);

            ProfileController controller = new ProfileController(profileRepositoryMock.Object);
            var results = controller.Get("test") as IEnumerable<CurrentLiftsViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }



    }
}
