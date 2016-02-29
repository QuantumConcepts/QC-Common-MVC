using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QuantumConcepts.Common.Mvc.Providers
{
    public class ExtendedDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public const string Key_GroupName = "GroupName";

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            ModelMetadata modelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            DisplayAttribute displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute != null)
                modelMetadata.AdditionalValues[ExtendedDataAnnotationsModelMetadataProvider.Key_GroupName] = displayAttribute.GroupName;

            return modelMetadata;
        }
    }
}