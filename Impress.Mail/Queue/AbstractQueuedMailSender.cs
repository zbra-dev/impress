using Impress.Logging;
using Impress.Mail.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Impress.Mail.Queue
{
    public abstract class AbstractQueuedMailSender : IMailSenderProcess
    {

        private readonly object processLock = new object();
        private Thread thread = null;

        private bool enabled = true;
        private int queueRate = 5 * 60 * 1000; // 5 minutes in miliseconds;
        private int messageRate = 500; // 500 0.5 seconds in miliseconds;
        private int maxRetries = 5;
        private IMailSender realSender;

        public AbstractQueuedMailSender(QueueSenderConfiguration configuration, IMailSender realSender)
        {
            this.realSender = realSender;
            // defensive copy
            this.enabled = configuration.Enabled;
            this.queueRate = configuration.QueueRate;
            this.messageRate = configuration.MessageRate;
            this.maxRetries = configuration.MaxRetries;
        }

        protected int MaxRetries { get { return this.maxRetries; } }

        public abstract void Send(IMailMessage message);

        public void Start()
        {
            var log = this.Logger();
            lock (processLock)
            {
                if (thread != null)
                {
                    throw new InvalidOperationException("Thread is running");
                }
                if (enabled)
                {
                    log.Debug("Starting QueuedMailSender deamon...");
                    thread = new Thread(() => Loop());
                    thread.Name = "QueuedMailSender deamon";
                    thread.Start();
                }
                else
                {
                    log.Warn("Mailer Daemon is not enabled.");
                }
            }
        }

        public void Stop()
        {
            var log = this.Logger();
            lock (processLock)
            {
                if (thread != null)
                {
                    log.Debug("Stoping QueuedMailSender daemon...");
                    thread.Abort();
                    thread = null;
                }
                else
                {
                    log.Warn("Mailer daemon is not running");
                }
            }
        }

        private void Loop()
        {
            var log = this.Logger();
            log.Info("Mailer daemon started");
            while (true)
            {
                try
                {
                    ProcessQueue();
                    Thread.Sleep(queueRate);
                }
                catch (ThreadAbortException)
                {
                    // log.Info("Mailer daemon stop requested");
                    break;
                }
                catch (Exception)
                {
                    //  log.Fatal("Error on Mailer daemon. Waiting before resuming daemon processs", ex);
                    Thread.Sleep(queueRate);
                }
            }
            log.Info("Mailer daemon stopped");
        }

        private void ProcessQueue()
        {
            var log = this.Logger();
            log.Debug("Processing queue...");

            var queuedMessages = GetQueuedMessages();
            foreach (var msg in queuedMessages)
            {
                try
                {
                    log.Debug("Sending message [{0}]...", msg.Id);
                    realSender.Send(msg);
                    Dequeue(msg);
                    Thread.Sleep(messageRate); // avoid overload SMTP server
                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (TimeoutTransportMailException e)
                {
                    log.Debug(e, "Timeout sending message [{0}]. Retries: {1}. Retrying later.", msg.Id, msg.Retries);
                    msg.IncrementRetry();
                    Requeue(msg);
                }
                catch (MailException ex)
                {
                    log.Debug(ex, "Error sending message [{0}]. Retries: {1}. Retrying later.", msg.Id, msg.Retries);
                    msg.IncrementRetry();
                    Requeue(msg);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Unhandled Error sending message [{0}].", msg.Id);
                    throw ex;
                }
            }
        }

        protected abstract void Requeue(IQueableMailMessage msg);
        protected abstract void Dequeue(IQueableMailMessage msg);
        protected abstract IEnumerable<IQueableMailMessage> GetQueuedMessages();
    }
}
