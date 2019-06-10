namespace Impress.Validation
{
    public class NotNullValidator<T> : IValidator<T>
    {
        public NotNullValidator()
        {
            this.InvalidMessageKey = "Invalid.Is.Null";
        }

        public string InvalidMessageKey { get; set; }


        public ValidationResult Validate(T candidate)
        {

            var result = new ValidationResult();

            if (candidate == null)
            {
                result.AddReason(MessageInvalidationReason.Error(InvalidMessageKey));
            }

            return result;
        }
    }
}
