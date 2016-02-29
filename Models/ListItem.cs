using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuantumConcepts.Common.Mvc;
using QuantumConcepts.Common.Mvc.Attributes;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class ListItem : IComparable<ListItem>, IEquatable<ListItem>
    {
        public const int None = -1;

        [TemplateVisibility(false, false)]
        public bool CanHaveText { get; private set; }

        [TemplateVisibility(false, false)]
        public string Label { get; set; }

        [TemplateVisibility(false, false)]
        public object Value { get; set; }

        [TemplateVisibility(false, false)]
        public string Text { get; set; }

        [TemplateVisibility(false, false)]
        public Dictionary<string, string> Data { get; set; }

        public ListItem() { }

        public ListItem(string label, object value) : this(label, value, null, false) { }

        public ListItem(string label, object value, string text, bool canHaveText = false)
        {
            this.Label = label;
            this.Value = value;
            this.Text = text;
            this.CanHaveText = canHaveText;
        }

        public int CompareTo(ListItem other)
        {
            if (!string.Equals(this.Label, other.Label))
                return string.Compare(this.Label, other.Label);

            if (!string.Equals(this.Value, other.Value))
                return string.Compare(Convert.ToString(this.Value), Convert.ToString(other.Value));

            if (!string.Equals(this.Text, other.Text))
                return string.Compare(this.Text, other.Text);

            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ListItem))
                return false;

            return Equals((ListItem)obj);
        }

        public bool Equals(ListItem other)
        {
            return (this.CompareTo(other) == 0);
        }

        public override int GetHashCode()
        {
            return (this.Label.ValueOrDefault(o => o.GetHashCode()) ^ this.Value.ValueOrDefault(o => o.GetHashCode()) ^ this.Text.ValueOrDefault(o => o.GetHashCode()));
        }
    }
}