using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class SplitButtonModel
    {
        public SplitButtonItem DefaultButton { get; set; }
        public SplitButtonItem DropDownButton { get; set; }
        public List<SplitButtonItem> Items { get; set; }

        public SplitButtonModel()
        {
            this.Items = new List<SplitButtonItem>();
        }

        public SplitButtonModel(SplitButtonItem defaultItem, params SplitButtonItem[] items)
            : this(defaultItem, null, items)
        {
        }

        public SplitButtonModel(SplitButtonItem defaultItem, SplitButtonItem dropDownButton, params SplitButtonItem[] items)
            : this()
        {
            this.DefaultButton = defaultItem;
            this.DropDownButton = dropDownButton;

            if (!items.IsNullOrEmpty())
            {
                if (this.DropDownButton == null)
                    this.DropDownButton = new SplitButtonItem(null, "&nbsp;", null, "{ \"text\": false, \"icons\": { \"primary\": \"ui-icon-triangle-1-s\" } }");

                this.Items = new List<SplitButtonItem>(items);
            }
        }
    }
}