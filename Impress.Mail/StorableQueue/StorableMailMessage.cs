using System;
using System.Collections.Generic;

namespace Impress.Mail.StorableQueue
{
    /// <summary>
    /// Object to send to IMailStore. This object has a persistent friendy contract but is not intented to be persisted directly. Instead 
    /// the concrete implementation of IMailStore should copy this object  to the persistable object.
    /// </summary>
    public class StorableMailMessage : IQueableMailMessage
    {

        public long? Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime NextRetryDate { get; set; }
        public int Retries { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SystemKey { get; set; }
        public bool IsHtmlBody { get; set; }
        public string ReplyTo { get; set; }
        public IReadOnlyList<IMailAttachment> Attachments { get { return attachs; } }

        private List<StorableMailAttachment> attachs = new List<StorableMailAttachment>();

        public override bool Equals(object obj)
        {
            var other = obj as StorableMailMessage;
            return other != null && this.Id.HasValue && other.Id.HasValue && this.Id.Value == other.Id.Value;
        }

        public override int GetHashCode()
        {
            return Id.HasValue ? Id.Value.GetHashCode() : 0;
        }

        internal void Add(StorableMailAttachment storableMailAttachment)
        {
            attachs.Add(storableMailAttachment);
        }

        public void IncrementRetry()
        {
            this.Retries += 1;
        }
    }
}
