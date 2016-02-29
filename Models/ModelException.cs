using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class ModelException : Exception
    {
        public string Action { get; private set; }
        public string Controller { get; private set; }
        public object RouteValues { get; private set; }

        public ModelException(string message) : this(message, null, null, null) { }

        public ModelException(string message, string action) : this(message, action, null, null) { }

        public ModelException(string message, string action, object routeValues) : this(message, action, null, routeValues) { }

        public ModelException(string message, string action, string controller, object routeValues)
            : base(message)
        {
            this.Action = action;
            this.Controller = controller;
            this.RouteValues = routeValues;
        }
    }
}