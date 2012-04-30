using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace TheChallenge.Helpers
{
    internal class RequestContentReadPolicy : IRequestContentReadPolicy
    {
        public RequestContentReadKind GetRequestContentReadKind(System.Web.Http.Controllers.HttpActionDescriptor actionDescriptor)
        {
            return RequestContentReadKind.AsKeyValuePairsOrSingleObject;
        }
    }
}