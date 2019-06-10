using System;
using System.Text.RegularExpressions;

namespace Impress.Validation
{
    public class EmailPatternValidator : IValidator<String>
    {
        private static readonly Regex emailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public EmailPatternValidator()
        {
            this.InvalidMessageKey = "Email.Invalid.Message";
        }

        public string InvalidMessageKey { get; set; }

        public ValidationResult Validate(string email)
        {
            var result = new ValidationResult();
            if (email != null)
            {
                var match = emailRegex.Match(email);
                if (!match.Success)
                    result.AddReason(MessageInvalidationReason.Error(InvalidMessageKey, email));
            }
            return result;
        }
    }
}
