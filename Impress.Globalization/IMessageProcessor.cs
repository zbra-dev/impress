using System.Globalization;

namespace Impress.Globalization
{
    public interface IMessageProcessor
    {

        MessageTranslation Translate(CultureInfo culture, string key, object[] parameters);
    }
}
