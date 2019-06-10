using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Impress.Globalization
{
    public class Message
    {

        public static InCultureMesageBuilder InCulture(CultureInfo culture)
        {
            return new InCultureMesageBuilder(culture);
        }

        public static Message ForKey(string key, params object[] parameters)
        {
            return new InCultureMesageBuilder(null).ForKey(key, parameters);
        }

        public static Message ForKey(Enum value, params object[] parameters)
        {
            return new InCultureMesageBuilder(null).ForKey(value, parameters);
        }

        public string Label { get; private set; }
        public string Key { get; private set; }
        public IReadOnlyList<object> Parameters { get; private set; }

        public Message(string label)
        {
            this.Label = label;
            Parameters = new List<object>();
        }

        internal Message(string label, string key, object[] parameters)
        {
            Label = label;
            this.Key = key;
            this.Parameters = parameters;
        }

        public override string ToString()
        {
            return Label;
        }
    }

    public class InCultureMesageBuilder
    {

        private CultureInfo culture;

        public InCultureMesageBuilder(CultureInfo culture)
        {
            this.culture = culture;
        }

        public Message ForKey(string key, params object[] parameters)
        {
            return new Message(MessageTranslatorRegistry.GetMessageTranslator().Translate(culture, key, parameters), key, parameters);
        }

        public Message ForKey(Enum value, params object[] parameters)
        {
            return ForKey(ReadKeyForEnum(value), parameters);
        }

        private static string ReadKeyForEnum(Enum value)
        {
            var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var attribute = (MessageKey)memberInfo.GetCustomAttributes(typeof(MessageKey), false).FirstOrDefault();
                return attribute == null ? string.Empty : attribute.Key;
            }
            throw new ArgumentException(string.Format("Enum value {0} is not annotated with MessageKey attribute", value));
        }

    }
}
