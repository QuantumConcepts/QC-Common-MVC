using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantumConcepts.Common.Mvc.Attributes;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class HierarchicalListItem : ListItem
    {
        [TemplateVisibility(false, false)]
        public HierarchicalListItem Parent { get; set; }

        [TemplateVisibility(false, false)]
        public bool Selectable { get; set; }

        public List<HierarchicalListItem> Items { get; set; }

        [TemplateVisibility(false, false)]
        public HierarchicalListItem this[int index]
        {
            get
            {
                CheckRange(index);

                return this.Items[index];
            }
            set
            {
                CheckRange(index);

                this.Items[index] = value;
            }
        }

        public HierarchicalListItem()
        {
            this.Items = new List<HierarchicalListItem>();
        }

        public HierarchicalListItem(HierarchicalListItem parent, bool selectable, string label, string value)
            : this(parent, selectable, label, value, null, false) { }

        public HierarchicalListItem(HierarchicalListItem parent, bool selectable, string label, string value, string text, bool canHaveText = false)
            : base(label, value, text, canHaveText)
        {
            this.Parent = parent;
            this.Selectable = selectable;
            this.Items = new List<HierarchicalListItem>();
        }

        private void CheckRange(int index)
        {
            if (index < 0 || index > (this.Items.Count - 1))
                throw new IndexOutOfRangeException();
        }
    }
}