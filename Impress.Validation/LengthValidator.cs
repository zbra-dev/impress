using System;

namespace Impress.Validation
{
    public class LengthValidator<T> : IValidator<T>
    {
        private uint? min;
        private uint? max;

        public static LengthValidator<T> Max(uint max)
        {
            return new LengthValidator<T>(null, max);
        }

        public static LengthValidator<T> Min(uint min)
        {
            return new LengthValidator<T>(min, null);
        }

        public static LengthValidator<T> Interval(uint min, uint max)
        {
            return new LengthValidator<T>(min, max);
        }

        public LengthValidator(uint? min, uint? max)
        {
            this.min = min;
            this.max = max;
            this.InvalidMinMessageKey = "Invalid.Min.Length.Message";
            this.InvalidMaxMessageKey = "Invalid.Max.Length.Message";
        }

        public string InvalidMinMessageKey { get; set; }
        public string InvalidMaxMessageKey { get; set; }

        public ValidationResult Validate(T candidate)
        {
            var result = new ValidationResult();

            var length = ExtractLength(candidate);

            if (min.HasValue && length < min.Value)
            {
                result.AddReason(MessageInvalidationReason.Error(InvalidMinMessageKey, length, min));
            }
            if (max.HasValue && length > max.Value)
            {
                result.AddReason(MessageInvalidationReason.Error(InvalidMaxMessageKey, length, max));
            }

            return result;
        }

        private uint ExtractLength(T candidate)
        {
            var stringCandidate = candidate as String;

            if (stringCandidate != null)
            {
                return (uint)stringCandidate.Length;
            }

            return 0;
        }

    }
}
