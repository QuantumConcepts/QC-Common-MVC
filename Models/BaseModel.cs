using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuantumConcepts.Common.Mvc.Attributes;

namespace QuantumConcepts.Common.Mvc.Models
{
    public abstract class BaseModel
    {
        [HiddenInput]
        [TemplateVisibility(false, true)]
        public string ReturnUrl { get; set; }
    }
}