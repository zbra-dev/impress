using System;
using System.Collections.Generic;
using System.Text;

namespace Impress.Mail.StorableQueue
{
    public class StoredQueueMailSender : AbstractQueuedMailSender
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(StoredQueueMailSender));

        private IMailStore store;

        public StoredQueueMailSender(QueueSenderConfiguration configuration, IMailSender realSender, IMailStore store)
            : base(configuration, realSender)
        {
            this.store = store;
        }

        public override void Send(IMailMessage message)
        {
            try
            {
                // log.Debug("Storing mail message");
                store.Insert(CopyToStorableMailMessage(message));
                //log.Debug("Mail message stored");
            }
            catch (Exception e)
            {
                // log.Error("Error storing mail message", e);
                throw new MailException(e);
            }

        }

        protected virtual StorableMailMessage CopyToStorableMailMessage(IMailMessage other)
        {
            var storeMessage = new StorableMailMessage
            {
                CreationDate = DateTime.Now,
                NextRetryDate = DateTime.Now,
                From = other.From,
                To = other.To,
                Cc = other.Cc,
                Bcc = other.Bcc,
                Body = other.Body,
                IsHtmlBody = other.IsHtmlBody,
                ReplyTo = other.ReplyTo,
                Subject = other.Subject
            };

            foreach (var att in other.Attachments)
            {
                using (var memoryStream = new MemoryStream())
                {
                    att.GetContentStream().CopyTo(memoryStream);
                    storeMessage.Add(new StorableMailAttachment(storeMessage) { ContentType = att.GetContentType(), Content = memoryStream.ToArray() });
                }
            }

            return storeMessage;
        }

        protected override void Requeue(IQueableMailMessage msg)
        {
            // TODO change next retry date
            store.Update((StorableMailMessage)msg);
        }

        protected override void Dequeue(IQueableMailMessage msg)
        {
            store.Delete((StorableMailMessage)msg);
        }

        protected override System.Collections.Generic.IEnumerable<IQueableMailMessage> GetQueuedMessages()
        {
            return store.RetriveMessages(this.MaxRetries, 100);
        }
    }
}
