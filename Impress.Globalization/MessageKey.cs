using System;

namespace Impress.Globalization
{
    /// <summary>
    /// Use this Attribute in enums to automaticly provide a message key for each enum value.
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Field)]
    public class MessageKey : Attribute
    {
        public string Key { get; private set; }

        public MessageKey(string key)
        {
            Key = key;
        }
    }
}
