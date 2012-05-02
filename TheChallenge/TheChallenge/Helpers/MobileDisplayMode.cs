using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using System.Collections.Specialized;

namespace TheChallenge.Helpers
{
    public class MobileDisplayMode : DefaultDisplayMode
    {
        private readonly StringCollection _useragenStringPartialIdentifiers = new StringCollection
    {
        "Android",
        "Mobile",
        "Opera Mobi",
        "Samsung",
        "HTC",
        "Nokia",
        "Ericsson",
        "SonyEricsson",
        "iPhone"
    };

        public MobileDisplayMode()
            : base("Mobile")
        {
            ContextCondition = (context => IsMobile(context.GetOverriddenUserAgent()));
        }

        private bool IsMobile(string useragentString)
        {
            return _useragenStringPartialIdentifiers.Cast<string>()
                        .Any(val => useragentString.IndexOf(val, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }
    }
}