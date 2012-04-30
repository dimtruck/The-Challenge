using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Common;

namespace TheChallenge.Helpers
{
    public class CustomLogger : ILogger
    {
        public void Log(string category, System.Diagnostics.TraceLevel level, Func<string> messageCallback)
        {
            //throw new NotImplementedException();
        }

        public void LogException(string category, System.Diagnostics.TraceLevel level, Exception exception)
        {
            //throw new NotImplementedException();
        }
    }
}