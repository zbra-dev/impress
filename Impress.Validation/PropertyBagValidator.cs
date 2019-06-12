using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Impress.Validation
{
    public class PropertyBagValidator<T> : CompositeValidator<T>
    {

        private readonly IDictionary<string, object> propertyValidators = new Dictionary<string, object>();

        protected void AddAgnosticPropertyValidator(Type propertyType, string propertyName, object propertyValidator)
        {
            object composite;
            if (!propertyValidators.TryGetValue(propertyName, out composite))
            {
                Type type = typeof(PropertyValidator<>).MakeGenericType(propertyType);
                composite = Activator.CreateInstance(type, new object[] { typeof(T).Name, propertyName });
                propertyValidators.Add(propertyName, composite);
            }

            composite.GetType().GetMethod("Add").Invoke(composite, new object[] { propertyValidator });
        }

        private PropertyBagValidator<T> DoAddPropertyValidator<P>(string propertyName, IValidator<P> propertyValidator, params IValidator<P>[] adicionalPropertyValidators)
        {
            PropertyValidator<P> compositeValidator;
            object composite;
            if (!propertyValidators.TryGetValue(propertyName, out composite))
            {
                compositeValidator = new PropertyValidator<P>(typeof(T).Name, propertyName);
                propertyValidators.Add(propertyName, compositeValidator);
            }
            else
            {
                compositeValidator = (PropertyValidator<P>)composite;
            }

            compositeValidator.Add(propertyValidator);

            foreach (IValidator<P> validator in adicionalPropertyValidators)
            {
                compositeValidator.Add(validator);
            }

            return this;
        }

        public PropertyBagValidator<T> AddPropertyValidator<P>(Expression<Func<T, P>> expression, IValidator<P> propertyValidator, params IValidator<P>[] adicionalPropertyValidators)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new Exception("Property Selector does not return a property");
            }
            if (memberExpression.Expression.NodeType != ExpressionType.Parameter)
            {
                throw new Exception("Property Selector must return a immediate property");
            }
            return DoAddPropertyValidator(memberExpression.Member.Name, propertyValidator, adicionalPropertyValidators); ;
        }

        public new PropertyBagValidator<T> Add(IValidator<T> validator)
        {
            base.Add(validator);
            return this;
        }

        public new PropertyBagValidator<T> Remove(IValidator<T> validator)
        {
            base.Remove(validator);
            return this;
        }


        public override ValidationResult Validate(T candidate)
        {

            var result = new ValidationResult();

            if (propertyValidators.Count > 0)
            {
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(candidate);

                    // IValidator<> validator
                    object propertyValidator;
                    if (propertyValidators.TryGetValue(property.Name, out propertyValidator))
                    {
                        var propertyResult = (ValidationResult)propertyValidator.GetType().GetMethod("Validate").Invoke(propertyValidator, new object[] { value });

                        result = result.Merge(propertyResult);

                    }
                }
            }

            return result.Merge(base.Validate(candidate));
        }

    }
}
