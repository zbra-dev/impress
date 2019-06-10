using System;

namespace Impress.Validation.Annotations
{
    public interface IValidationAttribute
    {
        object GetValidator(Type propertyType);
    }
}
