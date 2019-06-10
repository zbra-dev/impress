using System;

namespace Impress.Validation.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LengthAttribute : Attribute, IValidationAttribute
    {
        public uint? Min { get; private set; }
        public uint? Max { get; private set; }

        public LengthAttribute(uint min, uint max)
        {
            Min = min;
            Max = max;
        }

        public LengthAttribute(uint max)
        {
            Min = null;
            Max = max;
        }

        public object GetValidator(Type propertyType)
        {
            Type type = typeof(LengthValidator<>).MakeGenericType(propertyType);
            object typedValidator = null;
            if (Min.HasValue && Max.HasValue)
            {
                typedValidator = type.GetMethod("Interval").Invoke(null, new object[] { Min.Value, Max.Value });
            }
            else if (Min.HasValue)
            {
                typedValidator = type.GetMethod("Min").Invoke(null, new object[] { Min.Value });
            }
            else if (Max.HasValue)
            {
                typedValidator = type.GetMethod("Max").Invoke(null, new object[] { Max.Value });
            }

            return typedValidator;
        }
    }
}
