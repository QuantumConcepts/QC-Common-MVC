using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class BlockButton : ListItem
    {
        public string Description { get; set; }

        public BlockButton() { }

        public BlockButton(string label, string description, string value)
            : base(label, value, null)
        {
            this.Description = description;
        }
    }
}