namespace Impress.Validation
{
    public static class ValidationExtensions
    {
        public static void ValidateWith<T>(this T candidate, IValidator<T> validator)
        {
            var result = validator.Validate(candidate);

            if (!result.IsValid())
            {
                throw new ValidationException(result);
            }
        }

        public static void ValidateWith<T>(this T candidate, IValidator<T> validator, params IValidator<T>[] additionalValidators)
        {
            var result = validator.Validate(candidate);

            foreach (IValidator<T> additionalValidator in additionalValidators)
            {
                result = result.Merge(additionalValidator.Validate(candidate));
            }

            if (!result.IsValid())
            {
                throw new ValidationException(result);
            }
        }
    }
}
