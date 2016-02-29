using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QuantumConcepts.Common.Extensions;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class TableModel
    {
        public SplitButtonModel SplitButton { get; private set; }
        public IEnumerable<object> Items { get; private set; }
        public List<string> ColumnNames { get; private set; }
        public List<string> ColumnDisplayNames { get; private set; }
        public string IDPropertyName { get; private set; }

        public TableModel(SplitButtonModel splitButton, IEnumerable<object> items, string idPropertyName = "ID")
        {
            this.SplitButton = splitButton;
            this.Items = items;
            this.IDPropertyName = idPropertyName;

            Initialize();
        }

        public TableModel(IEnumerable<object> items, string idPropertyName = "ID") : this((string)null, items, idPropertyName) { }

        public TableModel(string editUrl, IEnumerable<object> items, string idPropertyName = "ID")
        {
            if (!editUrl.IsNullOrEmpty())
                this.SplitButton = new SplitButtonModel(new SplitButtonItem(editUrl, "Edit", null, null));

            this.Items = items;
            this.IDPropertyName = idPropertyName;

            Initialize();
        }

        private void Initialize()
        {
            if (!this.Items.IsNullOrEmpty())
            {
                object firstItem = this.Items.First();
                ViewDataDictionary<object> viewData = new ViewDataDictionary<object>();
                ModelMetadata metadata = ModelMetadata.FromLambdaExpression<object, object>(m => firstItem, viewData);
                List<ModelMetadata> properties = (from p in metadata.Properties
                                                  where
                                                      p.ShowForDisplay
                                                      && object.Equals(p.AdditionalValues.ValueOrDefault("ShowInTable"), true)
                                                  select p).ToList();

                //Sort the properties by the TableColumnOrder additional metadata value.
                properties = properties.OrderBy(o =>
                {
                    int? columnOrder = o.AdditionalValues.ValueOrDefault("TableColumnOrder").ValueOrDefault(x => (int?)Convert.ToInt32(x));

                    //If the TableColumnOrder exists, sort by that.
                    if (columnOrder.HasValue)
                        return columnOrder.Value;

                    //Otherwise, put it at the end of the list. This will keep unordered columns in their original order.
                    return int.MaxValue;
                }).ToList();

                this.ColumnNames = properties.Select(o => o.PropertyName).ToList();
                this.ColumnDisplayNames = properties.Select(o => (o.DisplayName ?? o.PropertyName)).ToList();
            }
        }

        public IEnumerable<IDictionary<PropertyInfo, object>> GetItemDictionaries()
        {
            foreach (object item in this.Items)
            {
                Dictionary<PropertyInfo, object> dictionary = new Dictionary<PropertyInfo, object>();

                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(item))
                {
                    if (this.ColumnNames.Contains(property.Name) || string.Equals(this.IDPropertyName, property.Name))
                    {
                        PropertyInfo propertyInfo = new PropertyInfo()
                        {
                            Name = property.Name,
                            DataType = (property.Attributes.OfType<DataTypeAttribute>().SingleOrDefault().ValueOrDefault(o => (o.CustomDataType ?? o.DataType.ToString())) ?? property.PropertyType.Name)
                        };

                        dictionary.Add(propertyInfo, property.GetValue(item));
                    }
                }

                yield return dictionary;
            }
        }

        public IEnumerable<PropertyValue> GetItemValues(IDictionary<PropertyInfo, object> item)
        {
            foreach (string columnName in this.ColumnNames)
                yield return GetItemValue(item, columnName);
        }

        public PropertyValue GetItemValue(IDictionary<PropertyInfo, object> item, string columnName)
        {
            KeyValuePair<PropertyInfo, object> column = item.SingleOrDefault(o => o.Key.Name.Equals(columnName));

            return new PropertyValue()
            {
                Info = column.Key,
                Value = column.Value
            };
        }

        public SplitButtonModel CreateSplitButtonForItem(IDictionary<PropertyInfo, object> item)
        {
            SplitButtonModel splitButton = new SplitButtonModel();

            splitButton.DefaultButton = CopySplitButtonItem(this.SplitButton.DefaultButton, item);
            splitButton.DropDownButton = CopySplitButtonItem(this.SplitButton.DropDownButton, item);

            foreach (SplitButtonItem original in this.SplitButton.Items)
                splitButton.Items.Add(CopySplitButtonItem(original, item));

            return splitButton;
        }

        private SplitButtonItem CopySplitButtonItem(SplitButtonItem original, IDictionary<TableModel.PropertyInfo, object> item)
        {
            if (original != null)
            {
                string href = original.Href;

                if (href.IsNullOrEmpty())
                    return original;
                
                //Update the URL, filling in any values from this item. Replace {ColumnName} or the encoded version %7BColumnName%7D.
                foreach (string columnName in this.ColumnNames.Union(new[] { this.IDPropertyName }))
                    href = Regex.Replace(href, "({{{0}}}|%7B{0}%7D)".FormatString(columnName), Convert.ToString(GetItemValue(item, columnName).Value));

                return new SplitButtonItem(href, original.Text, original.Class, original.Options);
            }

            return null;
        }

        public struct PropertyInfo
        {
            public string Name { get; set; }
            public string DataType { get; set; }
        }

        public struct PropertyValue
        {
            public PropertyInfo Info { get; set; }
            public object Value { get; set; }
        }
    }
}