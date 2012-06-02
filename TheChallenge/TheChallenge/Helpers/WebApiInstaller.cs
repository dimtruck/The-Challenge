﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using System.Web.Http.Dispatcher;
using System.Web.Http;
using Domain;
using Domain.Repository;
using System.Web.WebPages;
using Domain.Factory.Interfaces;
using Domain.Factory;

namespace TheChallenge.Helpers
{
    internal class WebApiInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, 
            Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            String connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["TheChallenge"].ConnectionString;

            container.Register(
                Component.For<MiniProfilerInterceptor>().
                    LifestyleTransient(),
                Component.For<IContestFactory>().
                    ImplementedBy<ContestFactory>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    LifestylePerWebRequest(),
                Component.For<IWorkoutFactory>().
                    ImplementedBy<WorkoutFactory>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    LifestylePerWebRequest(),
                Component.For<IExerciseFactory>().
                    ImplementedBy<ExerciseFactory>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    LifestylePerWebRequest(),
                Component.For<IFoodFactory>().
                    ImplementedBy<FoodFactory>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    LifestylePerWebRequest(),
                Component.For<IMealFactory>().
                    ImplementedBy<MealFactory>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    LifestylePerWebRequest(),
                Component.For<IProfileFactory>().
                    ImplementedBy<ProfileFactory>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    LifestylePerWebRequest(),
                Component.For<IContestRepository>().
                    ImplementedBy<ContestRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IMealRepository>().
                    ImplementedBy<MealRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IUserRepository>().
                    ImplementedBy<UserRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IFoodRepository>().
                    ImplementedBy<FoodRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IProfileRepository>().
                    ImplementedBy<ProfileRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IWorkoutRepository>().
                    ImplementedBy<WorkoutRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IExerciseRepository>().
                    ImplementedBy<ExerciseRepository>().
                    Interceptors(typeof(MiniProfilerInterceptor)).
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IHttpControllerFactory>().
                    ImplementedBy<WindsorControllerFactory>().
                    LifestyleSingleton(),
                Component.For<System.Web.Http.Common.ILogger>().
                    ImplementedBy<CustomLogger>().
                    LifestyleSingleton(),
                Component.For<System.Web.Http.ModelBinding.IRequestContentReadPolicy>().
                    ImplementedBy<RequestContentReadPolicy>().
                    LifestyleSingleton(),
                Component.For<System.Net.Http.Formatting.IFormatterSelector>().
                    ImplementedBy<System.Net.Http.Formatting.FormatterSelector>().
                    LifestyleSingleton(),
                Component.For<IHttpControllerActivator>().
                    ImplementedBy<DefaultHttpControllerActivator>().
                    LifestyleTransient(),
                Component.For<System.Web.Http.Controllers.IHttpActionSelector>().
                    ImplementedBy<System.Web.Http.Controllers.ApiControllerActionSelector>().
                    LifestyleTransient(),
                Component.For<System.Web.Http.Controllers.IActionValueBinder>().
                    ImplementedBy<System.Web.Http.ModelBinding.DefaultActionValueBinder>().
                    LifestyleTransient(),
                Component.For<System.Web.Http.Controllers.IHttpActionInvoker>().
                    ImplementedBy<System.Web.Http.Controllers.ApiControllerActionInvoker>().
                    LifestyleTransient(),
                Component.For<System.Web.Http.Metadata.ModelMetadataProvider>().
                    ImplementedBy<System.Web.Http.Metadata.Providers.CachedDataAnnotationsModelMetadataProvider>().
                    LifestyleTransient(),
                Component.For<System.Web.WebPages.IDisplayMode>().
                    ImplementedBy<System.Web.WebPages.DefaultDisplayMode>().
                    LifestyleTransient(),
                Component.For<HttpConfiguration>().
                Instance(GlobalConfiguration.Configuration));

        }
    }
}