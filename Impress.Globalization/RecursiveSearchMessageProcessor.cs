using System;
using System.Globalization;

namespace Impress.Globalization
{
    public class RecursiveSearchMessageProcessor : IMessageProcessor
    {

        private IMessageProcessor originalMessageProcessor;

        public RecursiveSearchMessageProcessor(IMessageProcessor messageProcessor)
        {
            originalMessageProcessor = messageProcessor;
        }

        public MessageTranslation Translate(CultureInfo culture, string key, object[] parameters)
        {

            var messageTranslation = originalMessageProcessor.Translate(culture, key, parameters);

            if (!messageTranslation.IsTranslated)
            {
                var workKey = key;

                while (!messageTranslation.IsTranslated)
                {
                    var pos = workKey.IndexOf('.');
                    if (pos < 0)
                    {
                        break;
                    }
                    workKey = workKey.Substring(pos + 1);
                    messageTranslation = originalMessageProcessor.Translate(culture, workKey, parameters);
                }

                if (!messageTranslation.IsTranslated)
                {
                    return new MessageTranslation() { IsTranslated = false, MessageKey = key, MessageParams = parameters };
                }
            }

            return messageTranslation;
        }

    }
}
