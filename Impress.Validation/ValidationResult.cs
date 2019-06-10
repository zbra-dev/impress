using System;
using System.Collections.Generic;
using System.Linq;

namespace Impress.Validation
{
    [Serializable]
    public class ValidationResult
    {
        private IDictionary<InvalidationSeverity, IList<IInvalidationReason>> reasonsBySeverity = new Dictionary<InvalidationSeverity, IList<IInvalidationReason>>();

        public ValidationResult AddReason(IInvalidationReason reason)
        {

            IList<IInvalidationReason> reasons;
            if (!reasonsBySeverity.TryGetValue(reason.Severity, out reasons))
            {
                reasons = new List<IInvalidationReason>();
                reasonsBySeverity.Add(reason.Severity, reasons);
            }

            reasons.Add(reason);
            return this;
        }

        /// <summary>
        /// True if no Error severity reaosns are present
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !reasonsBySeverity.ContainsKey(InvalidationSeverity.Error) || reasonsBySeverity[InvalidationSeverity.Error].Count == 0;
        }

        /// <summary>
        /// True if contains no invalidation reason at all.
        /// </summary>
        /// <returns></returns>
        public bool IsStriclyValid()
        {
            return reasonsBySeverity.Count == 0;
        }

        public IEnumerable<IInvalidationReason> Reasons()
        {
            return reasonsBySeverity.Values.SelectMany(r => r);
        }

        public IEnumerable<IInvalidationReason> Reasons(InvalidationSeverity severity)
        {
            IList<IInvalidationReason> reasons;
            if (!reasonsBySeverity.TryGetValue(severity, out reasons))
            {
                return new IInvalidationReason[0];
            }
            return reasons;
        }

        public ValidationResult Merge(ValidationResult other)
        {
            if (this.reasonsBySeverity.Count == 0)
            {
                return other;
            }
            else if (other.reasonsBySeverity.Count == 0)
            {
                return this;
            }

            var newResult = new ValidationResult();
            foreach (IInvalidationReason reason in other.Reasons().Concat(this.Reasons()))
            {
                newResult.AddReason(reason);
            }

            return newResult;
        }
    }
}
