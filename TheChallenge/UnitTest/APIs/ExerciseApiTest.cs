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
    class ExerciseApiTest
    {
        private Mock<IExerciseRepository> exerciseRepositoryMock = new Mock<IExerciseRepository>();
        private Mock<IWorkoutRepository> workoutRepositoryMock = new Mock<IWorkoutRepository>();
        private SaveWorkoutViewModel saveWorkoutViewModel = new SaveWorkoutViewModel()
        {
            EntryDate = DateTime.Now,
            Exercises = new List<SaveExerciseViewModel>(){
                new SaveExerciseViewModel(){
                    Name = "ex1",
                    Distance = 0,
                    Id = 0,
                    Reps = 0,
                    Time = "00:00:00.000",
                    Weight = 0
                },
                new SaveExerciseViewModel(){
                    Name = "ex2",
                    Distance = 0,
                    Id = 0,
                    Reps = 0,
                    Time = "00:00:00.000",
                    Weight = 0
                }
            }
        };

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<SaveExerciseViewModel, ExerciseEntry>()
                .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Reps, opt => opt.MapFrom(src => src.Reps))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeSpan.Parse(src.Time)))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            AutoMapper.Mapper.CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EventName));
        }

        [TearDown]
        public void TearDown()
        {
            exerciseRepositoryMock.VerifyAll();
            workoutRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetExercisesTest()
        {
            exerciseRepositoryMock.Setup(t => t.RetrieveExercises()).Returns(new List<Event>(){
                new Event(){ EventName = "test"},
                new Event() { EventName = "test2"},
                new Event() { EventName = "test3"}
            });

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<EventViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("test", results.First(t => t.Name.Equals("test")).Name);
        }

        [Test]
        public void GetExerciseNoneReturnTest()
        {
            IList<Event> events = null;
            exerciseRepositoryMock.Setup(t => t.RetrieveExercises()).Returns(events);

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<EventViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void AddNewWorkoutTest()
        {
            workoutRepositoryMock.Setup(t => t.SaveWorkout(It.IsAny<Workout>())).Returns(true);

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Post(saveWorkoutViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.Created, results.StatusCode);
        }

        [Test]
        public void AddNewWorkoutNullRequestTest()
        {
            SaveWorkoutViewModel model = null;

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Post(model) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("no valid request content present", results.ReasonPhrase);
        }

        [Test]
        public void AddNewWorkoutNoExercisesTest()
        {
            SaveWorkoutViewModel model = new SaveWorkoutViewModel();
            model.EntryDate = DateTime.Now;

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Post(model) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("no exercises present", results.ReasonPhrase);
        }

        [Test]
        public void AddNewWorkoutNoEntryDateTest()
        {
            SaveWorkoutViewModel model = new SaveWorkoutViewModel();
            model.Exercises = saveWorkoutViewModel.Exercises;

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Post(model) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("no valid date present", results.ReasonPhrase);
        }

        [Test]
        public void AddNewWorkoutUnsuccessfullySavedTest()
        {
            workoutRepositoryMock.Setup(t => t.SaveWorkout(It.IsAny<Workout>())).Returns(false);

            ExerciseController controller = new ExerciseController(exerciseRepositoryMock.Object, workoutRepositoryMock.Object);
            var results = controller.Post(saveWorkoutViewModel) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("unable to save workout", results.ReasonPhrase);
        }
    }
}
