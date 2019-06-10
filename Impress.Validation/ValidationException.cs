using System;

namespace Impress.Validation
{
    [Serializable]
    public class ValidationException : Exception
    {
        private ValidationResult result;

        public ValidationException(ValidationResult result)
        {
            this.result = result;
        }

        public ValidationResult GetValidationResult()
        {
            return result;
        }
    }
}
