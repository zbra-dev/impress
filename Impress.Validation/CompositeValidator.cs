using System.Collections.Generic;

namespace Impress.Validation
{
    public class CompositeValidator<T> : IValidator<T>
    {
        private ICollection<IValidator<T>> validators = new HashSet<IValidator<T>>();

        public CompositeValidator() { }

        public virtual CompositeValidator<T> Add(IValidator<T> validator)
        {
            validators.Add(validator);
            return this;
        }

        public virtual CompositeValidator<T> Remove(IValidator<T> validator)
        {
            validators.Remove(validator);
            return this;
        }

        public virtual ValidationResult Validate(T candidate)
        {
            var result = new ValidationResult();

            foreach (IValidator<T> validator in validators)
            {
                result = result.Merge(validator.Validate(candidate));
            }

            return result;
        }
    }
}
