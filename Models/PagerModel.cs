using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class PagerModel
    {
        public string Action { get; private set; }
        public string Controller { get; private set; }
        public string SingluarDescription { get; private set; }
        public string PluralDescription { get; private set; }
        public int Count { get; private set; }
        public int Page { get; private set; }
        public int PageCount { get; private set; }

        public PagerModel(string action, string controller, string singularDescription, string pluralDescription, int count, int page, int pageCount)
        {
            this.Action = action;
            this.Controller = controller;
            this.SingluarDescription = singularDescription;
            this.PluralDescription = pluralDescription;
            this.Count = count;
            this.Page = page;
            this.PageCount = pageCount;
        }

        public RouteValueDictionary GetRouteValues(ViewContext viewContext, int page)
        {
            RouteValueDictionary dictionary = viewContext.RouteData.Values;

            foreach (string key in viewContext.ViewData.ModelState.Keys)
                dictionary[key] = viewContext.ViewData.ModelState[key].Value.ValueOrDefault(o => o.AttemptedValue);

            dictionary["Page"] = page;

            return dictionary;
        }
    }
}