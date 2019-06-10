using System.Collections.Generic;

namespace Impress.Globalization
{
    public class CompositeMessageProcessor : IMessageProcessor
    {
        private readonly List<IMessageProcessor> processors = new List<IMessageProcessor>();

        public void AddProcessor(IMessageProcessor processor)
        {
            processors.Add(processor);
        }

        public void RemoveProcessor(IMessageProcessor processor)
        {
            processors.Remove(processor);
        }

        public MessageTranslation Translate(System.Globalization.CultureInfo culture, string key, object[] parameters)
        {
            foreach (var processor in processors)
            {
                var translation = processor.Translate(culture, key, parameters);
                if (translation.IsTranslated)
                    return translation;
            }
            return new MessageTranslation()
            {
                IsTranslated = false,
                MessageKey = key,
                MessageParams = parameters
            };
        }
    }
}
