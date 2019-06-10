using System;
using System.Linq;

namespace Impress.Globalization
{
    /// <summary>
    /// Represents the preference for a given culture.
    /// Ia multi-language enviroment may be necessary to locate all possible cultures 
    /// the system can handle and sort then based on a user by user configuration
    /// </summary>
    public class CulturePreference : IComparable
    {
        public double Preference { get; set; }
        public string Language { get; set; }

        public int CompareTo(object obj)
        {
            return Preference.CompareTo(((CulturePreference)obj).Preference);
        }

        private string GetLanguageCode()
        {
            return Language.Contains("-") ? Language.Split('-')[0] : Language;
        }

        private string GetCountryCode()
        {
            return Language.Contains("-") ? Language.Split('-')[1] : null;
        }

        /// <summary>
        /// 0 - not compatilbe with any, 0.5 - only language compatible with some , 1 - language and country compatible with some
        /// </summary>
        /// <param name="systemPreferences"></param>
        /// <returns>float</returns>
        public float ScoreCompatible(CulturePreference[] systemPreferences)
        {
            if (systemPreferences.Any(p => p.GetLanguageCode() == GetLanguageCode() && p.GetCountryCode() == p.GetCountryCode()))
            {
                return 1f;
            }
            else if (systemPreferences.Any(p => p.GetLanguageCode() == GetLanguageCode()))
            {
                return 0.5f;
            }
            return 0;
        }
    }
}
