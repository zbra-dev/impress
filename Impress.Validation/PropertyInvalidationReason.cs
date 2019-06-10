using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Validation
{
    public class PropertyInvalidationReason : IInvalidationReason
    {
        private string propertyName;
        private string typeName;
        private string originalInvalidMessageKey;
        private string messsageKey;
        private object[] messageParameters;
        private InvalidationSeverity severity;


        public PropertyInvalidationReason(string typeName, string propertyName, string messageKey, params object[] parameters) :
            this(typeName, propertyName, MessageInvalidationReason.Error(messageKey, parameters))
        { }

        public PropertyInvalidationReason(string typeName, string propertyName, IInvalidationReason valueReason)
        {
            this.typeName = typeName;
            this.propertyName = propertyName;
            this.severity = valueReason.Severity;
            var propertyReason = valueReason as PropertyInvalidationReason;
            if (propertyReason != null)
            {
                this.originalInvalidMessageKey = propertyReason.originalInvalidMessageKey;
                this.messsageKey = string.Format("{0}.{1}.{2}.{3}", typeName, propertyName, propertyReason.propertyName, originalInvalidMessageKey);
            }
            else
            {
                this.originalInvalidMessageKey = valueReason.MessageKey;
                this.messsageKey = string.Format("{0}.{1}.{2}", typeName, propertyName, valueReason.MessageKey);
            }

            messageParameters = valueReason.MessageParameters;
        }

        public InvalidationSeverity Severity
        {
            get { return severity; }
        }

        public string MessageKey
        {
            get
            {
                return messsageKey;
            }
        }

        public object[] MessageParameters
        {
            get
            {
                return messageParameters;
            }
        }
    }
}
