using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuantumConcepts.Common.Mvc.Attributes {
    public class DateRangeAttribute : RangeAttribute {
        public DateRangeAttribute(int? minYear = null, int? minMonth = null, int? minDay = null, int? maxYear = null, int? maxMonth = null, int? maxDay = null)
            : base(typeof(DateTime), OptionalDateToString(minYear, minMonth, minDay), OptionalDateToString(maxYear, maxMonth, maxDay)) {
        }

        private static string OptionalDateToString(int? year, int? month, int? day) {
            if (year.HasValue && month.HasValue && day.HasValue)
                return new DateTime(year.Value, month.Value, day.Value).ToShortDateString();

            return null;
        }
    }

    /// <summary>Specifies a minimum date of 1/1/1973 with no maximum.</summary>
    public class DefaultDateRangeAttribute : DateRangeAttribute {
        public DefaultDateRangeAttribute()
            : base(1973, 1, 1) {
        }
    }
}