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
    class WorkoutApiTest
    {
        private Mock<IWorkoutRepository> workoutRepositoryMock = new Mock<IWorkoutRepository>();
        private Mock<IWorkoutFactory> workoutFactoryMock = new Mock<IWorkoutFactory>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<ExerciseEntry, SaveExerciseViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExerciseId))
                .ForMember(dest => dest.Reps, opt => opt.MapFrom(src => src.Reps))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.HasValue ? src.Time.Value.ToString() : string.Empty))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        }

        [TearDown]
        public void TearDown()
        {
            workoutRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetWorkoutDatesTest()
        {
            workoutRepositoryMock.Setup(t => t.GetWorkoutDates()).Returns(new List<DateTime>(){
                new DateTime(2010,1,1),
                new DateTime(2011,1,1),
                new DateTime(2012,1,1)
            });

            WorkoutController controller = new WorkoutController(workoutRepositoryMock.Object, workoutFactoryMock.Object);
            var results = controller.Get() as IEnumerable<DateTime>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(new DateTime(2010,1,1)));

        }

        [Test]
        public void GetWorkoutDatesNoneTest()
        {
            IList<DateTime> dates = null;
            workoutRepositoryMock.Setup(t => t.GetWorkoutDates()).Returns(dates);

            WorkoutController controller = new WorkoutController(workoutRepositoryMock.Object, workoutFactoryMock.Object);
            var results = controller.Get() as IEnumerable<DateTime>;

            Assert.IsNull(results);
        }

        [Test]
        public void GetExercisesSavedForADayTest()
        {
            workoutRepositoryMock.Setup(t => t.GetWorkout(It.IsAny<DateTime>())).Returns(new Workout(){
                ExerciseEntries = new List<ExerciseEntry>(){
                    new ExerciseEntry(){
                        Distance = 1,
                        ExerciseId = 1,
                        Name = "bench",
                        Reps = 5,
                        Weight = 1
                    },
                    new ExerciseEntry(){
                        Distance = 1,
                        ExerciseId = 1,
                        Name = "squat",
                        Reps = 10,
                        Weight = 1
                    },
                    new ExerciseEntry(){
                        Distance = 1,
                        ExerciseId = 1,
                        Name = "golf",
                        Reps = 15,
                        Weight = 1
                    }
                },
                WorkoutDate = new DateTime(2010,1,1)
            });

            WorkoutController controller = new WorkoutController(workoutRepositoryMock.Object, workoutFactoryMock.Object);
            var results = controller.Get(DateTime.Now) as IEnumerable<SaveExerciseViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("golf", results.First(t => t.Reps == 15).Name);

        }

        [Test]
        public void GetExercisesSavedForADayNoneTest()
        {
            Workout workout = null;
            workoutRepositoryMock.Setup(t => t.GetWorkout(It.IsAny<DateTime>())).Returns(workout);

            WorkoutController controller = new WorkoutController(workoutRepositoryMock.Object, workoutFactoryMock.Object);
            var results = controller.Get(DateTime.Now) as IEnumerable<SaveExerciseViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }
    }
}
