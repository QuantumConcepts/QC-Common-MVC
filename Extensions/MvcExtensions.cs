using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.Utils;
using System.Web.Mvc.Html;
using QuantumConcepts.Common.Mvc.Models;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;
using System.Dynamic;
using QuantumConcepts.Common.Mvc.Providers;
using System.Linq.Expressions;
using System.ComponentModel;

namespace QuantumConcepts.Common.Mvc.Extensions {
    public static class MvcExtensions {
        private const string Key_Message = "Message";
        private const string Key_MessagePriority = "MessagePriority";
        private const string Key_List = "List";
        private const string Key_ReturnUrl = "ReturnUrl";

        public static object SetMessage(this TempDataDictionary dictionary, object value, string priority = null) {
            if (dictionary != null) {
                dictionary[Key_Message] = value;
                dictionary[Key_MessagePriority] = priority;
            }

            return value;
        }

        public static object GetMessage(this TempDataDictionary dictionary) {
            if (dictionary != null && dictionary.ContainsKey(Key_Message))
                return dictionary[Key_Message];

            return null;
        }

        public static object GetMessagePriority(this TempDataDictionary dictionary) {
            if (dictionary != null && dictionary.ContainsKey(Key_MessagePriority))
                return dictionary[Key_MessagePriority];

            return null;
        }

        public static bool HasMessage(this TempDataDictionary dictionary) {
            if (dictionary != null && dictionary.ContainsKey(Key_Message))
                return true;

            return false;
        }

        public static void AddQueryString(this RouteValueDictionary dictionary, NameValueCollection querystring) {
            if (dictionary == null || querystring.IsNullOrEmpty())
                return;

            foreach (string key in querystring.Keys)
                dictionary[key] = querystring[key];
        }

        public static bool HasListItemData(this IDictionary<string, object> dictionary) {
            return dictionary.ContainsKey(Key_List);
        }

        public static List<ListItem> GetListItemDataFromParent(this IDictionary<string, object> additionalValues, ViewDataDictionary parentViewData) {
            //If list is null, try to pull the list from the AdditionalMetaData.
            if (additionalValues.HasListItemData()) {
                string key = (string)additionalValues[Key_List];
                ModelMetadata property = parentViewData.ModelMetadata.Properties.SingleOrDefault(p => p.PropertyName.Equals(key));
                List<ListItem> list = (property.ValueOrDefault(o => o.Model) as List<ListItem>);

                return list;
            }

            return null;
        }

        public static List<ListItem> GetListItemData(this ViewDataDictionary viewData) {
            //If list is null, try to pull the list from the AdditionalMetaData.
            if (viewData.HasListItemData()) {
                List<ListItem> list = (viewData[Key_List] as List<ListItem>);

                return list;
            }

            return null;
        }

        public static ModelMetadata PropertyFor(this ModelMetadata modelMetadata, string propertyName) {
            return modelMetadata.Properties.SingleOrDefault(p => p.PropertyName.Equals(propertyName));
        }

        public static ExpandoObject ToExpando(this IDictionary<string, object> anonymousDictionary) {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var item in anonymousDictionary)
                expando.Add(item);

            return (ExpandoObject)expando;
        }

        public static ExpandoObject ToExpando(this object anonymousObject) {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(anonymousObject.GetType()))
                expando.Add(property.Name, property.GetValue(anonymousObject));

            return expando as ExpandoObject;
        }

        public static ViewDataDictionary ToViewDataDictionary(this object anonymousObject) {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            ViewDataDictionary viewDataDictionary = new ViewDataDictionary();

            //Copy the anonymous dictionary to the ViewDataDictionary.
            anonymousDictionary.Keys.ForEach(k => viewDataDictionary.Add(k, anonymousDictionary[k]));

            return viewDataDictionary;
        }

        public static string GetGroupName(this ModelMetadata modelMetadata) {
            if (modelMetadata.AdditionalValues.ContainsKey(ExtendedDataAnnotationsModelMetadataProvider.Key_GroupName))
                return (modelMetadata.AdditionalValues[ExtendedDataAnnotationsModelMetadataProvider.Key_GroupName] as string);

            return null;
        }

        public static string GetFieldId<T, V>(this HtmlHelper<T> html, Expression<Func<T, V>> selector) {
            return html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(selector));
        }

        public static string GetFieldName<T, V>(this HtmlHelper<T> html, Expression<Func<T, V>> selector) {
            return html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(selector));
        }

        public static string ToHtmlAttributesString(this IDictionary<string, object> attributes) {
            StringBuilder allAttributes = new StringBuilder();

            foreach (string key in attributes.Keys)
                allAttributes.AppendFormat(" {0}=\"{1}\"", key, attributes[key].ToString().Replace("\"", "'"));

            return allAttributes.ToString();
        }

        public static List<ListItem> GetYesNoListItems() {
            return new List<ListItem>()
            {
                new ListItem("Yes", bool.TrueString, null),
                new ListItem("No", bool.FalseString, null)
            };
        }

        public static List<ListItem> InsertAllListItem(this List<ListItem> items, int insertAt = 0) {
            return items.InsertEmptyListItem("(All)", insertAt: insertAt);
        }

        public static List<ListItem> InsertNoneListItem(this List<ListItem> items, int insertAt = 0) {
            return items.InsertEmptyListItem(insertAt: insertAt);
        }

        public static List<ListItem> InsertEmptyListItem(this List<ListItem> items, string label = "(None)", string value = null, int insertAt = 0) {
            items.Insert(insertAt, new ListItem(label, value, null));

            return items;
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, FormMethod method) {
            return htmlHelper.BeginForm(htmlHelper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString(), htmlHelper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(), method);
        }

        public static MvcForm BeginForm(this HtmlHelper htmlHelper, string actionName) {
            return htmlHelper.BeginForm(actionName, htmlHelper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString());
        }

        public static string AbsoluteAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null, string protocol = "http") {
            return url.Action(actionName, controllerName, routeValues, protocol);
        }

        public static bool HasReturnUrl(this ModelStateDictionary modelState) {
            return (modelState.ContainsKey(Key_ReturnUrl) && modelState[Key_ReturnUrl].ValueOrDefault(o => o.Value.ValueOrDefault(x => x.AttemptedValue)) != null);
        }

        public static string GetReturnUrl(this ModelStateDictionary modelState) {
            if (modelState.HasReturnUrl())
                return modelState[Key_ReturnUrl].ValueOrDefault(o => o.Value.ValueOrDefault(x => x.AttemptedValue));

            return null;
        }

        public static string GetActionName(this ViewContext viewContext) {
            return GetActionName(viewContext.Controller);
        }

        public static string GetActionName(this ControllerBase controller) {
            return (controller.ValueProvider.GetValue("action").RawValue as string);
        }

        public static string GetControllerName(this ViewContext viewContext) {
            return GetControllerName(viewContext.Controller);
        }

        public static string GetControllerName(this ControllerBase controller) {
            return (controller.ValueProvider.GetValue("controller").RawValue as string);
        }

        public static string GetAreaName(this ViewContext viewContext) {
            return GetAreaName(viewContext.Controller);
        }

        public static string GetAreaName(this ControllerBase controller) {
            //Return the current area. If it is null, return an empty string because that is how the default area is specified.
            return ((controller.ControllerContext.RouteData.DataTokens.ValueOrDefault("area") as string) ?? string.Empty);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues) {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, null);
        }

        public static string AbsoluteUrl(this UrlHelper url, string relativePath) {
            return url.Content(relativePath);
        }
    }
}