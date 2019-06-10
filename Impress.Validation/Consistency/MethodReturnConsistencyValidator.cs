using Impress.Validation.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Impress.Validation.Consistency
{
    public class MethodReturnConsistencyValidator : IValidator<MethodReturnData>
    {
        public ValidationResult Validate(MethodReturnData candidate)
        {
            var result = new ValidationResult();
            var attributes = candidate.Method.ReturnParameter.GetCustomAttributes(true).OfType<Attribute>();

            if (!attributes.Any() && (candidate.Method.ReturnParameter.ParameterType.IsArray || candidate.Method.ReturnParameter.ParameterType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>))))
            {
                attributes = new List<Attribute>() { new NotNullAttribute() };
            }

            foreach (var attr in attributes)
            {
                var attribute = attr as IValidationAttribute;

                if (attribute != null)
                {
                    var validator = attribute.GetValidator(candidate.Method.ReturnParameter.ParameterType);
                    result = result.Merge((ValidationResult)validator.GetType().GetMethod("Validate").Invoke(validator, new object[] { candidate.ReturnValue }));
                }
            }

            return result;
        }

    }

    public class MethodReturnData
    {
        public MethodInfo Method { get; private set; }
        public object ReturnValue { get; private set; }

        public MethodReturnData(MethodInfo method, object returnValue)
        {
            this.Method = method;
            this.ReturnValue = returnValue;
        }
    }
}
