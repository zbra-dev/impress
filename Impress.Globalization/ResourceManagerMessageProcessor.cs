using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace Impress.Globalization
{
    public class ResourceManagerMessageProcessor : IMessageProcessor
    {
        private readonly ResourceManager resourceManager;

        public ResourceManagerMessageProcessor(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        public MessageTranslation Translate(CultureInfo culture, string key, object[] parameters)
        {
            try
            {
                var translationCandidate = resourceManager.GetObject(key, culture);
                if (translationCandidate == null)
                {
                    return new MessageTranslation()
                    {
                        Translation = null,
                        IsTranslated = false,
                        MessageKey = key,
                        MessageParams = parameters
                    };
                }
                else
                {
                    var translation = translationCandidate.ToString();
                    if (parameters.Length > 0)
                    {
                        translation = PluralPreProcessing(translation, parameters);
                        return new MessageTranslation()
                        {
                            Translation = string.Format(culture, translation, parameters),
                            IsTranslated = true,
                            MessageKey = key,
                            MessageParams = parameters
                        };
                    }
                    return new MessageTranslation()
                    {
                        Translation = translation,
                        IsTranslated = true,
                        MessageKey = key,
                        MessageParams = parameters
                    };
                }
            }
            catch (MissingManifestResourceException)
            {
                return new MessageTranslation()
                {
                    Translation = null,
                    IsTranslated = false,
                    MessageKey = key,
                    MessageParams = parameters
                };
            }
        }

        private string PluralPreProcessing(string translation, object[] parameters)
        {
            var regex = new Regex(@"\{[^}]*\|{1}[^}]*\}");
            var translationBuilder = new StringBuilder(translation);
            foreach (Match match in regex.Matches(translation))
            {
                var words = match.Value.Split('|', ':', '{', '}');
                translationBuilder.Replace(
                    match.Value, 
                    "{" + words[1] + "} " + (ConvertToIndex(words[1]) == 1 ? words[2] : words[3])
                );
            }
            return translationBuilder.ToString();
        }

        private int? ConvertToIndex(string index)
        {
            if (!string.IsNullOrWhiteSpace(index) && int.TryParse(index, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
