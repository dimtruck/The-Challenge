using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http.Dispatcher;
using TheChallenge.Helpers;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using TheChallenge.Controllers;
using System.Web.Http.Controllers;
using Castle.Windsor.Configuration.Interpreters;
using Domain.Entities;
using TheChallenge.Models;
using System.Web.WebPages;

namespace TheChallenge
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "CurrentLifts",
                routeTemplate: "api/profile/current",
                defaults: new { controller = "profile", current = "current" }
            );

            routes.MapHttpRoute(
                name: "Contests",
                routeTemplate: "api/contest",
                defaults: new { controller = "contest" }
            );

            routes.MapHttpRoute(
                name: "Register",
                routeTemplate: "api/account",
                defaults: new { controller = "account" }
            );

            routes.MapHttpRoute(
                name: "SignOut",
                routeTemplate: "api/account",
                defaults: new { controller = "account" }
            );

            routes.MapHttpRoute(
                name: "SignIn",
                routeTemplate: "api/signin",
                defaults: new { controller = "signin" }
            );

            routes.MapHttpRoute(
                name: "ContestEvents",
                routeTemplate: "api/contest/{id}",
                defaults: new { controller = "contest", id = RouteParameter.Optional }
            );

            routes.MapHttpRoute(
                name: "Exercises",
                routeTemplate: "api/exercise",
                defaults: new { controller = "exercise" }
            );

            routes.MapHttpRoute(
                name: "Foods",
                routeTemplate: "api/food/{id}",
                defaults: new { controller = "food", id= RouteParameter.Optional }
            );

            routes.MapHttpRoute(
                name: "SaveFood",
                routeTemplate: "api/food",
                defaults: new { controller = "food"}
            );

            routes.MapHttpRoute(
                name: "Goals",
                routeTemplate: "api/goal",
                defaults: new { controller = "goal" }
            );

            routes.MapHttpRoute(
                name: "SaveExercise",
                routeTemplate: "api/exercise",
                defaults: new { controller = "exercise" }
            );

            routes.MapHttpRoute(
                name: "WorkoutDates",
                routeTemplate: "api/workout/{entryDate}",
                defaults: new { controller = "workout", entryDate = RouteParameter.Optional }
            );

            routes.MapHttpRoute(
                name: "MealDates",
                routeTemplate: "api/meal/{entryDate}",
                defaults: new { controller = "meal", entryDate = RouteParameter.Optional }
            );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            var configuration = GlobalConfiguration.Configuration;
            var container = new WindsorContainer(new XmlInterpreter()).Install(new WebApiInstaller());

            foreach (Type controllerType in typeof(HomeController).Assembly.GetTypes().Where(type => typeof(IHttpController).IsAssignableFrom(type)))
            {
                string name = controllerType.Name.Replace("Controller",String.Empty).ToLower();
                container.Register(Component
                    .For(controllerType)
                    .Named(name)
                    .LifestylePerWebRequest());
            }

            GlobalConfiguration.Configuration.ServiceResolver.SetResolver(
                serviceType => container.Resolve(serviceType),
                serviceType => container.ResolveAll(serviceType).Cast<object>()); 
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //BundleTable.Bundles.RegisterTemplateBundles();
            BundleTable.Bundles.EnableDefaultBundles();

            RegisterMappers();

            DisplayModeProvider.Instance.Modes.Insert(0, new MobileDisplayMode());



        }

        private void RegisterMappers()
        {
            AutoMapper.Mapper.CreateMap<Contest, ContestViewModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.ContestDetails))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContestName))
                .ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.ContestPlace));

            AutoMapper.Mapper.CreateMap<ContestViewModel, Contest>()
                .ForMember(dest => dest.ContestDetails, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.ContestName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContestPlace, opt => opt.MapFrom(src => src.Place));

            AutoMapper.Mapper.CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EventName));

            AutoMapper.Mapper.CreateMap<EventViewModel, Event>()
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id));

            AutoMapper.Mapper.CreateMap<SaveExerciseViewModel, ExerciseEntry>()
                .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Reps, opt => opt.MapFrom(src => src.Reps))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => TimeSpan.Parse(src.Time)))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            AutoMapper.Mapper.CreateMap<ExerciseEntry, SaveExerciseViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExerciseId))
                .ForMember(dest => dest.Reps, opt => opt.MapFrom(src => src.Reps))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time.HasValue?src.Time.Value.ToString():string.Empty))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            AutoMapper.Mapper.CreateMap<CurrentStatistic, CurrentLiftsViewModel>()
                .ForMember(dest => dest.DateLifted, opt => opt.MapFrom(src => src.LastExecuted))
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Exercise))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventGoalType))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result));

            AutoMapper.Mapper.CreateMap<ContestEvent, ContestEventViewModel>()
                .ForMember(dest => dest.EventGoal, opt => opt.ResolveUsing<ContestEventDataResolver>());
            AutoMapper.Mapper.CreateMap<ContestEventGoal, ContestEventGoalViewModel>()
                .ForMember(dest => dest.Result, opt => opt.ResolveUsing<GoalDataResolver>());
            AutoMapper.Mapper.CreateMap<Nutrient, NutrientViewModel>();
            AutoMapper.Mapper.CreateMap<Serving, ServingViewModel>();
            AutoMapper.Mapper.CreateMap<Food, FoodViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Int32.Parse(src.Id)));

            AutoMapper.Mapper.CreateMap<Meal, SaveFoodViewModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.MealDate))
                .ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.Foods));
            AutoMapper.Mapper.CreateMap<SaveFoodViewModel, FoodEntry>()
                .ForMember(dest => dest.ServingId, opt => opt.MapFrom(src => src.ServingTypeId.ToString()))
                .ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodId.ToString()));

            AutoMapper.Mapper.CreateMap<FoodEntry, MealEntryViewModel>()
                .ForMember(dest => dest.EntryDate, opt => opt.ResolveUsing<MealEntryDateResolver>());


        }
    }
}