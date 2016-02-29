using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuantumConcepts.Common.Mvc.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequirmentAttribute : RequiredAttribute {
        public bool Required { get; set; }

        public RequirmentAttribute() : base() { }

        public RequirmentAttribute(bool required)
            : base() {
            this.Required = required;
        }

        public override bool IsValid(object value) {
            return (!this.Required || base.IsValid(value));
        }
    }
}