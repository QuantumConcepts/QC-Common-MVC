using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class SplitButtonItem
    {
        public string Href { get; set; }
        public string Text { get; set; }
        public string Class { get; set; }
        public string Options { get; set; }

        public SplitButtonItem() { }

        public SplitButtonItem(string href, string text, string @class = null, string options = null)
        {
            this.Href = href;
            this.Text = text;
            this.Class = @class;
            this.Options = options;
        }
    }
}