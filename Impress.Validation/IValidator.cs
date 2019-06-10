using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Validation
{
    /// <summary>
    /// Applies validation rules to the candidate. 
    /// </summary>
    /// <typeparam name="T">The type of the candicate this validator can validate.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Applies validation rules to the candidate. 
        /// If the candidate is null, the validation result should be valid. Exception to this rules applies only if the validation is intended to determine if the candidate is null.
        /// This allows for the composition of different validators in an independent maner.
        /// </summary>
        /// <param name="candidate">hte candidate object to be validated ( it can be null)</param>
        /// <returns>A ValidationResult with the resulting InvalidationReasons, if any. Cannot return null</returns>
        ValidationResult Validate(T candidate);
    }
}
