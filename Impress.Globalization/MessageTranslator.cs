using System.Globalization;

namespace Impress.Globalization
{
    public class MessageTranslator
    {
        private readonly IMessageProcessor processor;

        public CultureInfo Culture { get; private set; }

        public MessageTranslator(CultureInfo defaultCulture, IMessageProcessor processor)
        {
            Culture = defaultCulture;
            this.processor = processor;
        }

        public string Translate(CultureInfo culture, string key, params object[] parameters)
        {

            if (culture == null)
            {
                culture = Culture;
            }

            var translation = processor.Translate(culture, key, parameters);
            if (translation.IsTranslated)
            {
                return translation.Translation;
            }
            else
            {
                return string.Format("?{0}@{1}?", key, culture.ToString());
            }
        }

        public string Translate(string key, params object[] parameters)
        {
            return Translate(Culture, key, parameters);
        }
    }
}
