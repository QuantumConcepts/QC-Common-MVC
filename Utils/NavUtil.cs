using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using QuantumConcepts.Common.Mvc.Extensions;
using QuantumConcepts.Common.Extensions;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Web.Routing;

namespace QuantumConcepts.Common.Mvc.Utils {
    public static class NavUtil {
        public static string SelectedCssClass = "Selected";

        public static MvcHtmlString WriteMenuLink(this HtmlHelper html, string text, string action, string controller = null, string area = null, string cssClass = null, bool suppressSelection = false, object routeValues = null) {
            string areaToUse = (area ?? html.ViewContext.GetAreaName());
            string controllerToUse = (controller ?? html.ViewContext.GetControllerName());
            bool selected = (!suppressSelection && string.Equals(areaToUse, html.ViewContext.GetAreaName()) && string.Equals(controllerToUse, html.ViewContext.GetControllerName()));

            return WriteLink(html, text, action, controllerToUse, areaToUse, cssClass, selected, routeValues);
        }

        public static MvcHtmlString WriteSubMenuLink(this HtmlHelper html, string text, string action, string controller = null, string area = null, string cssClass = null, object routeValues = null) {
            string areaToUse = (area ?? html.ViewContext.GetAreaName());
            string controllerToUse = (controller ?? html.ViewContext.GetControllerName());
            bool selected = (string.Equals(areaToUse, html.ViewContext.GetAreaName()) && string.Equals(controllerToUse, html.ViewContext.GetControllerName()) && string.Equals(action, html.ViewContext.GetActionName()));

            return WriteLink(html, text, action, controllerToUse, areaToUse, cssClass, selected, routeValues);
        }

        private static MvcHtmlString WriteLink(HtmlHelper html, string text, string action, string controller = null, string area = null, string cssClass = null, bool selected = false, object routeValues = null) {
            List<string> cssClasses = new List<string>();

            if (selected)
                cssClasses.Add(NavUtil.SelectedCssClass);

            if (!cssClass.IsNullOrEmpty())
                cssClasses.Add(cssClass);

            return WriteMenuLink(html, text, action, controller, area, routeValues, cssClasses.ToArray());
        }

        private static MvcHtmlString WriteMenuLink(HtmlHelper html, string text, string action, string controller, string area, object routeValues = null, params string[] cssClasses) {
            RouteValueDictionary allRouteValues = new RouteValueDictionary(routeValues);
            Dictionary<string, object> allHtmlAttributes = new Dictionary<string, object>();
            MvcHtmlString link = null;

            if (area != null)
                allRouteValues["area"] = area;

            if (!cssClasses.IsNullOrEmpty())
                allHtmlAttributes["class"] = string.Join(" ", cssClasses);

            link = LinkExtensions.ActionLink(html, text, action, controller, routeValues: allRouteValues, htmlAttributes: allHtmlAttributes);
            
            return link;
        }
    }
}