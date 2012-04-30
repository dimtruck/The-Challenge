using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dispatcher;
using System.Web.Http;
using Castle.MicroKernel;
using System.Web.Http.Controllers;

namespace TheChallenge.Helpers
{
    internal class WindsorControllerFactory : IHttpControllerFactory
    {
        private readonly HttpConfiguration httpConfiguration;
        private readonly IKernel kernel;

        public WindsorControllerFactory(HttpConfiguration configuration, IKernel kernel)
        {
            this.httpConfiguration = configuration;
            this.kernel = kernel;
        }

        public System.Web.Http.Controllers.IHttpController CreateController(System.Web.Http.Controllers.HttpControllerContext controllerContext, string controllerName)
        {
            var controller = this.kernel.Resolve<IHttpController>(controllerName);
            controllerContext.Controller = controller;
            controllerContext.ControllerDescriptor = new HttpControllerDescriptor(
                this.httpConfiguration, controllerName, controller.GetType());

            return controllerContext.Controller;
        }

        public void ReleaseController(System.Web.Http.Controllers.IHttpController controller)
        {
            this.kernel.ReleaseComponent(controller);
        }
    }
}