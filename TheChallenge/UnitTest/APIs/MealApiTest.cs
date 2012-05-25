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
    class MealApiTest
    {
        private Mock<IMealRepository> mealRepositoryMock = new Mock<IMealRepository>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<Nutrient, NutrientViewModel>();
            AutoMapper.Mapper.CreateMap<Serving, ServingViewModel>();
            AutoMapper.Mapper.CreateMap<FoodEntry, MealEntryViewModel>()
                .ForMember(dest => dest.EntryDate, opt => opt.ResolveUsing<MealEntryDateResolver>());
        }

        [TearDown]
        public void TearDown()
        {
            mealRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetMealsByDateExistsTest()
        {
            mealRepositoryMock.Setup(t => t.GetFoodEntries(It.IsAny<DateTime>())).Returns(new List<FoodEntry>(){
                new FoodEntry(){
                    CalculatedNutrients = new List<Nutrient>(){
                        new Nutrient(){
                            Description = "nut1"
                        },
                        new Nutrient(){
                            Description = "nut1"
                        },
                        new Nutrient(){
                            Description = "nut1"
                        }
                    },
                    Date = DateTime.Now,
                    FoodId = "1",
                    Name = "food1",
                    SelectedServing = new Serving(){ Amount = 1},
                    ServingId = "1",
                    ServingSize = 5,
                    Id = 1
                },
                new FoodEntry(){
                    CalculatedNutrients = new List<Nutrient>(){
                        new Nutrient(){
                            Description = "nut1"
                        },
                        new Nutrient(){
                            Description = "nut1"
                        },
                        new Nutrient(){
                            Description = "nut1"
                        }
                    },
                    Date = DateTime.Now,
                    FoodId = "1",
                    Name = "food2",
                    SelectedServing = new Serving(){ Amount = 1},
                    ServingId = "1",
                    ServingSize = 5,
                    Id  = 2
                }
            });

            MealController controller = new MealController(mealRepositoryMock.Object);
            var results = controller.Get(DateTime.Now) as IEnumerable<MealEntryViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("food2", results.First(t => t.Id == 2).Name);

        }

        [Test]
        public void GetMealsByDateDNETest()
        {
            IList<FoodEntry> foodEntries = null;
            mealRepositoryMock.Setup(t => t.GetFoodEntries(It.IsAny<DateTime>())).Returns(foodEntries);

            MealController controller = new MealController(mealRepositoryMock.Object);
            var results = controller.Get(DateTime.Now) as IEnumerable<MealEntryViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }
    }
}
