namespace Impress.Validation
{
    public static class ValidationExtentions
    {

        public static void ValidateWith<T>(this T candidate, IValidator<T> validator)
        {
            var result = validator.Validate(candidate);

            if (!result.IsValid())
            {
                throw new ValidationException(result);
            }
        }

        public static void ValidateWith<T>(this T candidate, IValidator<T> validator, params IValidator<T>[] adicionarValidators)
        {
            var result = validator.Validate(candidate);

            foreach (IValidator<T> adicionalValidator in adicionarValidators)
            {
                result = result.Merge(adicionalValidator.Validate(candidate));
            }

            if (!result.IsValid())
            {
                throw new ValidationException(result);
            }
        }
    }
}
