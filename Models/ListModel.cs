using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using QuantumConcepts.Common.Extensions;
using System.Web.Mvc;
using QuantumConcepts.Common.Mvc.Attributes;
using System.Web.Routing;
using QuantumConcepts.Common.Utils;
using QuantumConcepts.Common.Mvc.Extensions;
using System.Linq.Expressions;

namespace QuantumConcepts.Common.Mvc.Models
{
    public abstract class ListModel<SortFieldEnumType, ObjectType, ModelType>
        where SortFieldEnumType : struct
    {
        [TemplateVisibility(false, false)]
        protected readonly Dictionary<SortFieldEnumType, Dictionary<SortDirection, Func<IEnumerable<ObjectType>, IEnumerable<ObjectType>>>> SortFunctions = new Dictionary<SortFieldEnumType, Dictionary<SortDirection, Func<IEnumerable<ObjectType>, IEnumerable<ObjectType>>>>();

        [HiddenInput]
        [TemplateVisibility(false, true)]
        public int Page { get; set; }

        [TemplateVisibility(false, false)]
        public int TotalCount { get; set; }

        [TemplateVisibility(false, false)]
        public int? PageSize { get; set; }

        [TemplateVisibility(false, false)]
        public int PageCount
        {
            get
            {
                if (this.PageSize.HasValue)
                    return (int)System.Math.Ceiling(this.TotalCount / (decimal)this.PageSize.Value);

                return 1;
            }
        }

        [HiddenInput]
        [TemplateVisibility(false, true)]
        public SortFieldEnumType Sort { get; set; }

        [HiddenInput]
        [TemplateVisibility(false, true)]
        public SortDirection SortDirection { get; set; }

        [TemplateVisibility(false, false)]
        public List<ModelType> Items { get; protected set; }

        public ListModel() : this(null, null, null) { }

        public ListModel(int? page, SortFieldEnumType? sort, SortDirection? sortDirection)
        {
            InitializeSortFunctions();

            this.Page = (page.HasValue ? page.Value : 1);
            this.PageSize = ConfigUtil.Instance.PageSize;
            this.Sort = (sort.HasValue ? sort.Value : default(SortFieldEnumType));
            this.SortDirection = (sortDirection.HasValue ? sortDirection.Value : SortDirection.Ascending);
        }

        public virtual void InitializeSortFunctions() { }

        public virtual void Initialize() { }

        public IEnumerable<ObjectType> ApplySortingAndPaging(IEnumerable<ObjectType> dataSource)
        {
            IEnumerable<ObjectType> filteredAndSortedDataSource = dataSource;

            if (this.SortFunctions.ContainsKey(this.Sort) && this.SortFunctions[this.Sort].ContainsKey(this.SortDirection))
                filteredAndSortedDataSource = this.SortFunctions[this.Sort][this.SortDirection](dataSource);

            if (this.PageSize.HasValue)
            {
                int countBeforePaging = this.TotalCount;
                int lastPage = (int)System.Math.Ceiling(countBeforePaging / (decimal)this.PageSize.Value);

                lastPage = lastPage < 1 ? 1 : lastPage;

                //Before applying paging, check if the current page number is out of range for the current number of items.
                if (this.Page > lastPage)
                    this.Page = lastPage;

                filteredAndSortedDataSource = filteredAndSortedDataSource.Skip((this.Page - 1) * this.PageSize.Value).Take(this.PageSize.Value);
            }

            return filteredAndSortedDataSource;
        }

        public RouteValueDictionary GetSortAttributes(ViewContext viewContext, SortFieldEnumType sortField)
        {
            RouteValueDictionary dictionary = viewContext.RouteData.Values;
            string prefix = (viewContext.ViewData["Prefix"] as string);
            string pageAttributeName = (prefix.IsNullOrEmpty() ? "page" : "{0}Page".FormatString(prefix));
            string sortAttributeName = (prefix.IsNullOrEmpty() ? "sort" : "{0}Sort".FormatString(prefix));
            string sortDirectionAttributeName = (prefix.IsNullOrEmpty() ? "sortDirection" : "{0}SortDirection".FormatString(prefix));

            foreach (string key in viewContext.ViewData.Keys)
                dictionary[key] = viewContext.ViewData[key];

            dictionary[pageAttributeName] = this.Page.ToString();
            dictionary[sortAttributeName] = sortField.ToString();
            dictionary[sortDirectionAttributeName] = (object.Equals(sortField, this.Sort) && this.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending).ToString();

            return dictionary;
        }

        protected Expression<Func<ObjectType, bool>> DoAnyFieldsContain(IEnumerable<Expression<Func<ObjectType, string>>> fields, string search)
        {
            return fields.Select(o => o.BuildContainsExpression(new[] { search })).Or();
        }

        protected Expression<Func<ObjectType, bool>> DoAllFieldsContain(IEnumerable<Expression<Func<ObjectType, string>>> fields, string search)
        {
            return fields.Select(o => o.BuildContainsExpression(new[] { search })).And();
        }
    }
}