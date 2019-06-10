using System;

namespace Impress.Validation.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    public class NotNullAttribute : Attribute, IValidationAttribute
    {

        public object GetValidator(Type propertyType)
        {
            Type type = typeof(NotNullValidator<>).MakeGenericType(propertyType);
            return Activator.CreateInstance(type);
        }
    }
}
