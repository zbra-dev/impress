using System.Collections.Generic;

namespace Impress.Validation
{
    public class NotEmptyValidator<T> : IValidator<T>
    {

        public NotEmptyValidator()
        {
            this.InvalidMessageKey = "Invalid.Is.Empty";
        }

        public string InvalidMessageKey { get; set; }


        public ValidationResult Validate(T candidate)
        {
            var result = new ValidationResult();

            if (candidate == null)
            {
                result.AddReason(MessageInvalidationReason.Error(InvalidMessageKey));
                return result;
            }

            var stringCandidate = candidate as string;

            if (candidate is string && string.IsNullOrEmpty(stringCandidate))
            {
                result.AddReason(MessageInvalidationReason.Error(InvalidMessageKey));
            }

            var enumerableCandidate = candidate as IEnumerable<object>;

            if (enumerableCandidate != null && !enumerableCandidate.GetEnumerator().MoveNext())
            {
                result.AddReason(MessageInvalidationReason.Error(InvalidMessageKey));
            }

            return result;
        }
    }
}
