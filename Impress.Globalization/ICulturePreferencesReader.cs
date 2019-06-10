using System.Collections.Generic;

namespace Impress.Globalization
{
    public interface ICulturePreferencesReader
    {

        /// <summary>
        /// Retrive a list of CulturePreferences sorted by Preference in descending order ( most preferable first)
        /// </summary>
        /// <returns></returns>
        IList<CulturePreference> ReadPreferences();
    }
}
