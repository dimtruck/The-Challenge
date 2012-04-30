using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using System.Web.Http.Dispatcher;
using System.Web.Http;
using Domain;
using Domain.Repository;

namespace TheChallenge.Helpers
{
    internal class WebApiInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, 
            Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            String connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["TheChallenge"].ConnectionString;

            container.Register(
                Component.For<IContestRepository>().
                    ImplementedBy<ContestRepository>().
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IProfileRepository>().
                    ImplementedBy<ProfileRepository>().
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IWorkoutRepository>().
                    ImplementedBy<WorkoutRepository>().
                    DependsOn(Parameter.ForKey("connectionString").Eq(connectionString)).
                    LifestylePerWebRequest(),
                Component.For<IExerciseRepository>().
                    ImplementedBy<ExerciseRepository>().
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
                Component.For<HttpConfiguration>().
                Instance(GlobalConfiguration.Configuration));
        }
    }
}