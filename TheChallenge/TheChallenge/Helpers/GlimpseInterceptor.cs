using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Text;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace TheChallenge.Helpers
{
    public class GlimpseInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            using (GlimpseTimer.Start(GetMessage(invocation), invocation.TargetType.Namespace))
            {
                invocation.Proceed();
            }
        }

        private string GetMessage(IInvocation invocation)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Called: {0}.{1} (", invocation.TargetType.Name, invocation.Method.Name);
            foreach (var arg in invocation.Arguments)
            {
                string argDesc = arg == null ? "null" : arg.ToString();
                message.Append(argDesc);
            }
            message.Append(")");
            return message.ToString();
        }
    }
}