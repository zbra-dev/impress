using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Impress.Mail.Net
{
    public class SmtpMailSender : IMailSender
    {

        private readonly SmtpClient smtp = new SmtpClient();

        private bool enabled;

        public SmtpMailSender()
        {
            this.enabled = true;
        }

        public SmtpMailSender(NetSenderConfiguration configuration)
        {
            // defensive copy
            this.enabled = configuration.Enabled;
            smtp.EnableSsl = configuration.EnableSsl;
        }

        public void Send(IMailMessage message)
        {
            if (message == null)
            {
                throw new InvalidMailMessage("Mail Message is required");
            }

            if (!enabled)
            {
                //log.Warn("Sending e-mail is disabled. Ignoring message");
                return;
            }

            // log.Debug("Sending e-mail...");

            MailMessage mail;
            try
            {
                mail = BuildMailMessage(message);
            }
            catch (Exception e)
            {
                // log.Error("Mailer Message contains some invalid information and cannot be sent", e);
                throw new InvalidMailMessage(e);
            }

            try
            {
                smtp.Send(mail);
                // log.Debug("e-mail sent successfully!");
            }
            catch (SmtpException e)
            {
                // log.Error("Error sending e-mail", e);
                if (e.Message.Contains("timed out"))
                {
                    throw new TimeoutTransportMailException(e);
                }
                if (e.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst)
                {
                    throw new AuthenticationRequiredMailMessage(e);
                }
                if (e.StatusCode == SmtpStatusCode.GeneralFailure)
                {
                    throw new TransportMailMessageException(e);
                }
                else
                {
                    throw new TransportMailMessageException(e);
                }
            }
            catch (Exception e)
            {
                //log.Error("Error sending e-mail", e);
                throw new InvalidMailMessage(e);
            }

        }

        private MailMessage BuildMailMessage(IMailMessage msg)
        {
            var from = msg.From;
            var to = msg.To;
            var subject = msg.Subject;
            var body = msg.Body;

            var mail = new MailMessage(from, to, subject, body);
            mail.IsBodyHtml = msg.IsHtmlBody;

            if (!string.IsNullOrWhiteSpace(msg.ReplyTo))
            {
                mail.ReplyToList.Add(new MailAddress(msg.ReplyTo));
            }

            if (!string.IsNullOrWhiteSpace(msg.Cc))
            {
                mail.CC.Add(msg.Cc);
            }

            if (!string.IsNullOrWhiteSpace(msg.Bcc))
            {
                mail.Bcc.Add(msg.Bcc);
            }

            if (mail.IsBodyHtml)
            {
                mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html")));
            }

            if (msg.Attachments != null && msg.Attachments.Count > 0)
            {
                foreach (var attachment in msg.Attachments)
                {
                    var stream = attachment.GetContentStream() as MemoryStream;

                    if (stream == null)
                    {
                        var att = new Attachment(attachment.GetContentStream(), new ContentType(attachment.GetContentType()));
                        mail.Attachments.Add(att);
                    }
                    else
                    {
                        var mem = new MemoryStream(stream.ToArray());

                        var att = new Attachment(mem, new ContentType(attachment.GetContentType()));
                        mail.Attachments.Add(att);
                    }



                }
            }
            return mail;
        }
    }
}
