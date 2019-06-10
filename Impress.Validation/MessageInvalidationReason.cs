namespace Impress.Validation
{
    public class MessageInvalidationReason : IInvalidationReason
    {

        public static MessageInvalidationReason Error(string messageKey, params object[] parameters)
        {
            return new MessageInvalidationReason(InvalidationSeverity.Error, messageKey, parameters);
        }

        public static MessageInvalidationReason Warn(string messageKey, params object[] parameters)
        {
            return new MessageInvalidationReason(InvalidationSeverity.Warning, messageKey, parameters);
        }

        private MessageInvalidationReason(InvalidationSeverity severity, string messageKey, object[] parameters)
        {
            this.Severity = severity;
            this.MessageKey = messageKey;
            this.MessageParameters = parameters;
        }

        public InvalidationSeverity Severity { get; private set; }

        public string MessageKey { get; private set; }

        public object[] MessageParameters { get; private set; }
    }
}
