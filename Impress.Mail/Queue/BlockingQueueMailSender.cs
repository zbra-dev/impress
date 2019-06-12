using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Impress.Mail.Queue
{
    public class BlockingQueueMailSender : AbstractQueuedMailSender
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(BlockingQueueMailSender));

        private BlockingCollection<IQueableMailMessage> queue = new BlockingCollection<IQueableMailMessage>();

        public BlockingQueueMailSender(QueueSenderConfiguration configuration, IMailSender realSender) : base(configuration, realSender) { }

        public override void Send(IMailMessage message)
        {
            Enque(new QueueMessage(message));
        }

        private void Enque(IQueableMailMessage message)
        {
            try
            {
                //log.Debug("Adding mail message to queue");
                queue.Add(message);
                //log.Debug("Mail message queued");
            }
            catch (Exception e)
            {
                //log.Error("Error adding mail to queue", e);
                throw new MailException(e);
            }
        }

        protected override void Requeue(IQueableMailMessage msg)
        {
            Enque(msg);
        }

        protected override void Dequeue(IQueableMailMessage msg)
        {
            // already was dequeue on read. no-op
        }

        protected override System.Collections.Generic.IEnumerable<IQueableMailMessage> GetQueuedMessages()
        {
            while (queue.Count > 0)
            {
                yield return queue.Take();
            }
        }
    }
}
