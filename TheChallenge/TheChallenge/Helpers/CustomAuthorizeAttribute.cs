using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Domain.Repository;
using System.Security.Principal;
using TheChallenge.Helpers.Encryption;
using Domain.Entities;

namespace TheChallenge.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly IUserRepository repository;

        //TODO: this is complete crap.  I need this to DI!
        public CustomAuthorizeAttribute()
        {
            String connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["TheChallenge"].ConnectionString;
            this.repository = new UserRepository(connectionString);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
            else
                base.HandleUnauthorizedRequest(actionContext);
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            if (Authorize(actionContext.Request))
                return;
            else
                HandleUnauthorizedRequest(actionContext);
        }

        private bool Authorize(HttpRequestMessage request)
        {
            if (!request.Headers.Contains("TC-Authorization"))
                return false;
            else
            {
                string tcAuthorization = request.Headers.GetValues("TC-Authorization").FirstOrDefault();
                if (!String.IsNullOrEmpty(tcAuthorization) && !tcAuthorization.Equals("null"))
                {
                    try
                    {
                        String userPassword = Crypto.DecryptStringAES(tcAuthorization);
                        String[] userPasswordArray = userPassword.Split(':');
                        String userName = userPasswordArray[0];
                        String password = userPasswordArray[1];
                        User user = repository.UserExists(userName);
                        if (user != null)
                        {
                            if (Crypto.DecryptStringAES(user.Password).Equals(Crypto.DecryptStringAES(password)))
                                return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}