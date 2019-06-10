using System.Globalization;

namespace Impress.Globalization
{
    public interface ICultureSelector
    {
        CultureInfo SelectCulture(CulturePreference[] preferences);
    }
}
