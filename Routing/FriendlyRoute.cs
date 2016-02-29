using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Routing
{
    public class FriendlyRoute : Route
    {
        public const string TitleKey = "Title";

        public FriendlyRoute(string url, object defaults, object constraints, string[] namespaces)
            : base(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new MvcRouteHandler())
        {
            if (!namespaces.IsNullOrEmpty())
            {
                this.DataTokens = new RouteValueDictionary();
                this.DataTokens["Namespaces"] = namespaces;
            }
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (values.ContainsKey(FriendlyRoute.TitleKey))
            {
                string value = (values[FriendlyRoute.TitleKey] as string);

                if (value != null)
                {
                    value = Regex.Replace(value, @"\s", "-");
                    value = Regex.Replace(value, @"[^\w-]", "");
                    
                    values[FriendlyRoute.TitleKey] = value;
                }
            }

            return base.GetVirtualPath(requestContext, values);
        }
    }
}