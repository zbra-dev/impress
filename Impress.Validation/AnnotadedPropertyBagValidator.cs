using Impress.Validation.Annotations;
using System;
using System.Reflection;

namespace Impress.Validation
{
    public class AnnotadedPropertyBagValidator<T> : PropertyBagValidator<T>
    {

        public AnnotadedPropertyBagValidator()
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                IValidationAttribute attribute = GetAnnotation<NotNullAttribute>(property);
                if (attribute != null)
                {
                    this.AddAgnosticPropertyValidator(property.PropertyType, property.Name, attribute.GetValidator(property.PropertyType));
                }

                attribute = GetAnnotation<NotEmptyAttribute>(property);
                if (attribute != null)
                {
                    this.AddAgnosticPropertyValidator(property.PropertyType, property.Name, attribute.GetValidator(property.PropertyType));
                }

                if (IsAnnotationPresent<EmailAttribute>(property))
                {
                    this.AddAgnosticPropertyValidator(property.PropertyType, property.Name, new EmailPatternValidator());
                }

                attribute = GetAnnotation<LengthAttribute>(property);
                if (IsAnnotationPresent<LengthAttribute>(property))
                {
                    this.AddAgnosticPropertyValidator(property.PropertyType, property.Name, attribute.GetValidator(property.PropertyType));
                }
            }
        }

        private bool IsAnnotationPresent<A>(PropertyInfo propertyInfo) where A : Attribute
        {
            return propertyInfo.GetCustomAttributes(typeof(A), false).Length > 0;
        }

        private A GetAnnotation<A>(PropertyInfo propertyInfo) where A : Attribute
        {
            var all = propertyInfo.GetCustomAttributes(typeof(A), false);

            if (all.Length > 0)
            {
                return (A)all[0];
            }
            
            return null;
            
        }
    }
}
