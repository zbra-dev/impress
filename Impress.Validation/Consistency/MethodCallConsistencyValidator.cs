using Impress.Validation.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Impress.Validation.Consistency
{
    public class MethodCallConsistencyValidator : IValidator<MethodCallData>
    {
        public ValidationResult Validate(MethodCallData candidate)
        {
            var result = new ValidationResult();
            var parameters = candidate.Method.GetParameters();
            for (int p = 0; p < parameters.Length; p++)
            {
                var parameter = parameters[p];
                var attributes = parameter.GetCustomAttributes();

                if (!attributes.Any() && (parameter.ParameterType.IsArray || parameter.ParameterType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>))))
                {
                    attributes = new List<Attribute>() { new NotNullAttribute() };
                }

                foreach (var attr in attributes)
                {
                    var attribute = attr as IValidationAttribute;

                    if (attribute != null)
                    {
                        var validator = attribute.GetValidator(parameter.ParameterType);
                        result = result.Merge((ValidationResult)validator.GetType().GetMethod("Validate").Invoke(validator, new object[] { candidate.Arguments[p] }));
                    }
                }
            }

            return result;
        }
    }


    public class MethodCallData
    {
        public MethodInfo Method { get; private set; }
        public object[] Arguments { get; private set; }

        public MethodCallData(MethodInfo method, object[] arguments)
        {
            this.Method = method;
            this.Arguments = arguments;
        }
    }
}
