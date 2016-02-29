using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuantumConcepts.Common.Mvc.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LayoutAttribute : Attribute {
        public string GroupName { get; set; }
        public string CssClasses { get; set; }
        public int Columns { get; set; }
        public bool ClearBefore { get; set; }
        public string Help { get; set; }

        public LayoutAttribute() {
            this.Columns = 12;
        }
    }
}