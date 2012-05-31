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
using Domain.Factory.Interfaces;

namespace UnitTest.APIs
{
    [TestFixture]
    public class ContestApiTest
    {
        private Mock<IContestRepository> contestRepositoryMock = new Mock<IContestRepository>();
        private Mock<IContestFactory> contestFactoryMock = new Mock<IContestFactory>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<Contest, ContestViewModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.ContestDetails))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContestName))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.ContestPlace));
            AutoMapper.Mapper.CreateMap<ContestViewModel, Contest>()
                .ForMember(dest => dest.ContestDetails, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.ContestName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContestPlace, opt => opt.MapFrom(src => src.Place));
            AutoMapper.Mapper.CreateMap<ContestEvent, ContestEventViewModel>()
                .ForMember(dest => dest.EventGoal, opt => opt.ResolveUsing<ContestEventDataResolver>());
        }

        [TearDown]
        public void TearDown()
        {
            contestRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllContestsTest()
        {
            contestRepositoryMock.Setup(t => t.RetrieveContests()).Returns(new List<Contest>(){
                new Contest(){ ContestName = "test"},
                new Contest() { ContestName = "test2"},
                new Contest() { ContestName = "test3"}
            });

            ContestController controller = new ContestController(contestFactoryMock.Object, contestRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<ContestViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("test", results.First(t => t.Name.Equals("test")).Name);
        }

        [Test]
        public void GetContestsNoneReturningTest()
        {
            IList<Contest> contests = null;
            contestRepositoryMock.Setup(t => t.RetrieveContests()).Returns(contests);

            ContestController controller = new ContestController(contestFactoryMock.Object, contestRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<ContestViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void GetContestEventsForContestTest()
        {
            contestRepositoryMock.Setup(t => t.RetrieveContestEvents(It.IsAny<int>())).Returns(new List<ContestEvent>(){
                new ContestEvent(){ EventDescription = "test"},
                new ContestEvent() { EventDescription = "test2"},
                new ContestEvent() { EventDescription = "test3"}
            });

            ContestController controller = new ContestController(contestFactoryMock.Object, contestRepositoryMock.Object);
            var results = controller.Get(0) as IEnumerable<ContestEventViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("test", results.First(t => t.EventDescription.Equals("test")).EventDescription);
        }

        [Test]
        public void GetContestEventsForNonExistingContestTest()
        {
            IList<ContestEvent> contestEvents = null;
            contestRepositoryMock.Setup(t => t.RetrieveContestEvents(It.IsAny<int>())).Returns(contestEvents);

            ContestController controller = new ContestController(contestFactoryMock.Object, contestRepositoryMock.Object);
            var results = controller.Get(0) as IEnumerable<ContestEventViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }
    }
}
