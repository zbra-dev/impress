using System;
using System.Collections.Generic;

namespace Impress.Mail.Queue
{
    internal class QueueMessage : IQueableMailMessage
    {
        private IMailMessage wrapped;

        public long? Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime NextRetryDate { get; set; }
        public int Retries { get; set; }

        public string From { get { return wrapped.From; } }
        public string To { get { return wrapped.To; } }
        public string Cc { get { return wrapped.Cc; } }
        public string Bcc { get { return wrapped.Bcc; } }
        public string Subject { get { return wrapped.Subject; } }
        public string Body { get { return wrapped.Body; } }
        public bool IsHtmlBody { get { return wrapped.IsHtmlBody; } }
        public string ReplyTo { get { return wrapped.ReplyTo; } }
        public IReadOnlyList<IMailAttachment> Attachments { get { return wrapped.Attachments; } }



        public QueueMessage(IMailMessage other)
        {
            var now = DateTime.Now;
            this.wrapped = other;
            this.Id = now.Ticks;
            this.CreationDate = now;
            this.NextRetryDate = now;
            this.Retries = 0;

        }

        public override bool Equals(object obj)
        {
            var other = obj as QueueMessage;
            return other != null && this.Id.HasValue && other.Id.HasValue && this.Id.Value == other.Id.Value;
        }

        public override int GetHashCode()
        {
            return Id.HasValue ? Id.Value.GetHashCode() : 0;
        }


        public void IncrementRetry()
        {
            this.Retries += 1;
        }
    }
}
