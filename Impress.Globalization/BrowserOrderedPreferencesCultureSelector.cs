using System.Globalization;
using System.Linq;

namespace Impress.Globalization
{
    public class BrowserOrderedPreferencesCultureSelector : ICultureSelector
    {
        private readonly ICulturePreferencesReader systemPreferencesReader;

        public BrowserOrderedPreferencesCultureSelector(ICulturePreferencesReader systemPreferencesReader)
        {
            this.systemPreferencesReader = systemPreferencesReader;
        }

        public CultureInfo SelectCulture(CulturePreference[] preferences)
        {
            var systemPreferences = systemPreferencesReader.ReadPreferences().ToArray();

            var scoredPreferences = preferences.Select(p => new CulturePreference() {
                Language = p.Language,
                Preference = p.ScoreCompatible(systemPreferences)
            }).Where(p => p.Preference > 0)
            .OrderByDescending(p => p.Preference)
            .ToArray();

            return CultureInfo.CreateSpecificCulture(scoredPreferences.Length == 0 ? systemPreferences[0].Language : scoredPreferences[0].Language);
        }
    }
}
