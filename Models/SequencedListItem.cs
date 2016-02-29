using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class SequencedListItem : ListItem
    {
        public int Sequence { get; set; }

        public SequencedListItem() { }

        public SequencedListItem(string label, string value) : this(label, value, 0) { }

        public SequencedListItem(string label, string value, int sequence)
            : base(label, value, null, false)
        {
            this.Sequence = sequence;
        }
    }
}