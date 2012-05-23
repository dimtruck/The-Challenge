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
    class FoodApiTest
    {
        private Mock<IFoodRepository> foodRepositoryMock = new Mock<IFoodRepository>();
        private Mock<IMealRepository> mealRepositoryMock = new Mock<IMealRepository>();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            AutoMapper.Mapper.CreateMap<Nutrient, NutrientViewModel>();
            AutoMapper.Mapper.CreateMap<Serving, ServingViewModel>();
            AutoMapper.Mapper.CreateMap<Food, FoodViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Int32.Parse(src.Id)));
            AutoMapper.Mapper.CreateMap<SaveFoodViewModel, FoodEntry>()
                .ForMember(dest => dest.ServingId, opt => opt.MapFrom(src => src.ServingTypeId.ToString()))
                .ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodId.ToString()));

        }

        [TearDown]
        public void TearDown()
        {
            foodRepositoryMock.VerifyAll();
            mealRepositoryMock.VerifyAll();
        }

        [Test]
        public void GetAllFoodsTest()
        {
            foodRepositoryMock.Setup(t => t.RetrieveFoods()).Returns(new List<Food>(){
                new Food(){ Name = "test", Id = "1"},
                new Food() { Name = "test2", Id = "2"},
                new Food() { Name = "test3", Id = "3"}
            });

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<FoodViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("test", results.First(t => t.Name.Equals("test")).Name);

        }

        [Test]
        public void GetAllFoodsNoneReturnTest()
        {
            IList<Food> foods = null;
            foodRepositoryMock.Setup(t => t.RetrieveFoods()).Returns(foods);

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Get() as IEnumerable<FoodViewModel>;

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void GetFoodByIdTest()
        {
            foodRepositoryMock.Setup(t => t.RetrieveCompleteFood(It.IsAny<int>())).Returns(new Food()
            {
                Name = "test", 
                Id = "1",
                AvailableNutrients = new List<Nutrient>(){
                    new Nutrient(){ Description = "Protein"},
                    new Nutrient(){ Description = "Carbs"},
                    new Nutrient(){ Description = "Fat"}
                },
                AvailableServings = new List<Serving>(){
                    new Serving(){ Description = "cup"},
                    new Serving(){ Description = "oz"},
                    new Serving(){ Description = "lb"}
                }
            });

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Get(0) as FoodViewModel;

            Assert.IsNotNull(results);
            Assert.AreEqual("test", results.Name);
            Assert.AreEqual(3, results.Nutrients.Count);
            Assert.AreEqual(3, results.Servings.Count);
        }

        [Test]
        public void GetFoodByIdNotExistsTest()
        {
            Food food = null;
            foodRepositoryMock.Setup(t => t.RetrieveCompleteFood(It.IsAny<int>())).Returns(food);

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Get(0) as FoodViewModel;

            Assert.IsNotNull(results);
            Assert.IsNullOrEmpty(results.Name);
            Assert.IsNull(results.Nutrients);
            Assert.IsNull(results.Servings);
        }

        [Test]
        public void GetFoodByIdNoNutrientsTest()
        {
            foodRepositoryMock.Setup(t => t.RetrieveCompleteFood(It.IsAny<int>())).Returns(new Food()
            {
                Name = "test",
                Id = "1",
                AvailableServings = new List<Serving>(){
                    new Serving(){ Description = "cup"},
                    new Serving(){ Description = "oz"},
                    new Serving(){ Description = "lb"}
                }
            });

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Get(0) as FoodViewModel;

            Assert.IsNotNull(results);
            Assert.AreEqual("test", results.Name);
            Assert.IsNull(results.Nutrients);
            Assert.AreEqual(3, results.Servings.Count);
        }

        [Test]
        public void GetFoodByIdNoServingsTest()
        {
            foodRepositoryMock.Setup(t => t.RetrieveCompleteFood(It.IsAny<int>())).Returns(new Food()
            {
                Name = "test",
                Id = "1",
                AvailableNutrients = new List<Nutrient>(){
                    new Nutrient(){ Description = "Protein"},
                    new Nutrient(){ Description = "Carbs"},
                    new Nutrient(){ Description = "Fat"}
                }
            });

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Get(0) as FoodViewModel;

            Assert.IsNotNull(results);
            Assert.AreEqual("test", results.Name);
            Assert.AreEqual(3, results.Nutrients.Count);
            Assert.IsNull(results.Servings);

        }

        [Test]
        public void SaveFoodsTest()
        {
            IList<SaveFoodViewModel> saveFoodViewModels = new List<SaveFoodViewModel>()
            {
                new SaveFoodViewModel(){
                    Date = DateTime.Now,
                    FoodId = 1,
                    Name = "food",
                    ServingSize = 1,
                    ServingTypeId = 1
                },
                new SaveFoodViewModel(){
                    Date = DateTime.Now,
                    FoodId = 2,
                    Name = "food2",
                    ServingSize = 1,
                    ServingTypeId = 2
                }
            };
            mealRepositoryMock.Setup(t => t.SaveMeals(It.IsAny<IList<Meal>>())).Returns(true);

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Post(saveFoodViewModels) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.Created, results.StatusCode);

        }

        [Test]
        public void SaveFoodNullTest()
        {
            IList<SaveFoodViewModel> saveFoodViewModels = null;

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Post(saveFoodViewModels) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("no valid request content present", results.ReasonPhrase);
        }

        [Test]
        public void SaveFoodNoEntryDateTest()
        {
            IList<SaveFoodViewModel> saveFoodViewModels = new List<SaveFoodViewModel>()
            {
                new SaveFoodViewModel(){
                    FoodId = 1,
                    Name = "food",
                    ServingSize = 1,
                    ServingTypeId = 1
                },
                new SaveFoodViewModel(){
                    Date = DateTime.Now,
                    FoodId = 2,
                    Name = "food2",
                    ServingSize = 1,
                    ServingTypeId = 2
                }
            };

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Post(saveFoodViewModels) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("no valid date present", results.ReasonPhrase);
        }

        [Test]
        public void SaveFoodNoFoodsTest()
        {
            IList<SaveFoodViewModel> saveFoodViewModels = new List<SaveFoodViewModel>()
            {
                new SaveFoodViewModel(){
                    Date = DateTime.Now
                },
                new SaveFoodViewModel(){
                    Date = DateTime.Now
                }
            };

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Post(saveFoodViewModels) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.BadRequest, results.StatusCode);
            Assert.AreEqual("no foods present", results.ReasonPhrase);
        }

        [Test]
        public void UnsuccessfullySaveFoodsTest()
        {
            IList<SaveFoodViewModel> saveFoodViewModels = new List<SaveFoodViewModel>()
            {
                new SaveFoodViewModel(){
                    Date = DateTime.Now,
                    FoodId = 1,
                    Name = "food",
                    ServingSize = 1,
                    ServingTypeId = 1
                },
                new SaveFoodViewModel(){
                    Date = DateTime.Now,
                    FoodId = 2,
                    Name = "food2",
                    ServingSize = 1,
                    ServingTypeId = 2
                }
            };
            mealRepositoryMock.Setup(t => t.SaveMeals(It.IsAny<IList<Meal>>())).Returns(false);

            FoodController controller = new FoodController(foodRepositoryMock.Object, mealRepositoryMock.Object);
            var results = controller.Post(saveFoodViewModels) as HttpResponseMessage;

            Assert.IsNotNull(results);
            Assert.AreEqual(HttpStatusCode.InternalServerError, results.StatusCode);
            Assert.AreEqual("unable to save food", results.ReasonPhrase);

        }


    }
}
